using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    static CharManager _instance;

    public static CharManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CharManager>();
            return _instance;
        }
    }

    List<Transform> _enemyTrses = new List<Transform>();
    public List<Transform> _EnemyTrses => _enemyTrses;
    List<Transform> _playerTrses = new List<Transform>();
    public List<Transform> _PlayerTrses => _playerTrses;

    List<CharMove> _charMoves = new List<CharMove>();
    [Header("캐릭터 프리펩"), SerializeField] List<GameObject> _charPrefs;

    [Header("HP바 프리펩"), SerializeField] GameObject _barPref;

    [Header("캔버스"), SerializeField] Canvas _canvas;

    [Header("스킬 세터"), SerializeField] SkillSetter _skillSetter;

    public GameObject FindGoByType(eCHARTYPE type)
    {
        GameObject go = null;
        for (int i = 0; i < _charPrefs.Count; i++)
        {
            CharInfo charInfo = _charPrefs[i].GetComponent<CharInfo>();
            if (type == charInfo._CharType)
            {
                go = _charPrefs[i];
            }
        }
        return go;
    }
    public void AddCharMove(CharMove charMove)
    {
        _charMoves.Add(charMove);
    }

    public void AddPlayerTrses(Transform trs)
    {
        _playerTrses.Add(trs);
    }
    public void AddEnemyTrses(Transform trs)
    {
        _enemyTrses.Add(trs);
    }
    public void UpdatePath()
    {
        HexGrid grid = HexBoardMaker.Instance._hexGrid;
        
        foreach(CharMove charMove in _charMoves)
        {
            Pathfinding pathFinding = new Pathfinding(grid);
            charMove.SetPath();
            charMove.MoveCharacter();
        }    
    }
    public void SpawnBar(MovableUnit movable, CharHealth health, BasicAttack attack)
    {
        GameObject hpBarGo = Instantiate(_barPref, _canvas.transform);
        
        MoveBar moveHpBar = hpBarGo.GetComponent<MoveBar>();
        moveHpBar.SetCanvasTrs(_canvas);
        moveHpBar.SetAwakePos(movable.transform.position);

        movable.SetMoveHpBar(moveHpBar);

        health.SetHpBar(moveHpBar);
        attack.SetMpBar(moveHpBar._MpBar);


    }
    /*
    public void SetSkill(BasicAttack attack, eCHARTYPE type)
    {
        _skillSetter.SetSkill(attack, type);
        
        
    }
    */
}
