using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct sChar
{
    public eCHARTYPE _Type;
    public int _Level;
    public int _Count;


    public void AddCount()
    {
        _Count++;
    }
    public void ResetCount()
    {
        _Count = 0;
    }
    public void LevelUp()
    {
        _Level++;
    }
}
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

    Dictionary<eCHARTYPE, sChar> _collectedChars = new Dictionary<eCHARTYPE, sChar>();
    //Dictionary<eCHARTYPE, int> _countCollectedChar = new Dictionary<eCHARTYPE, int>();
    [Header("레벨 2"), SerializeField] int _level2Needs = 3;
    [Header("레벨 3"), SerializeField] int _level3Needs = 5;

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
    public void SpawnBar(MovableUnit movable, CharHealth health, BasicAttack attack, int level, eTILESTATE state)
    {
        GameObject hpBarGo = Instantiate(_barPref, _canvas.transform);
        
        MoveBar moveHpBar = hpBarGo.GetComponent<MoveBar>();
        moveHpBar.SetCanvasTrs(_canvas);
        moveHpBar.SetAwakePos(movable.transform.position);
        moveHpBar.SetLevelIcon(level);
        moveHpBar.SetHpBarColor(state);

        movable.SetMoveHpBar(moveHpBar);

        health.SetHpBar(moveHpBar);
        attack.SetMpBar(moveHpBar._MpBar);


    }
    public void CountCollectedChar(eCHARTYPE type) //
    {
        

        
        if (_collectedChars.ContainsKey(type))
        {
            int needs = (_collectedChars[type]._Level == 1) ? _level2Needs : _level3Needs;

            sChar tempChar = _collectedChars[type];
            tempChar.AddCount();
            _collectedChars[type] = tempChar;
            if (_collectedChars[type]._Count >= needs)
            {

                tempChar = _collectedChars[type];
                tempChar.LevelUp();
                tempChar.ResetCount();
                _collectedChars[type] = tempChar;
            }

        }
        else
        {
            sChar charInfo = new sChar();
            charInfo._Type = type;
            charInfo._Level = 1;
            charInfo._Count = 1;

            _collectedChars.Add(type, charInfo);
        }
    }
    public int GetLevel(eCHARTYPE type)
    {
        int level = 0;
        level = _collectedChars[type]._Level;
        return level;
    }

    public eCHARTYPE[] GetCollectedChars()
    {
        //int count = _collectedChars.Keys.Count;
        eCHARTYPE[] collectedChars = _collectedChars.Keys.ToArray();

        //collectedChars = _collectedChars.Keys.ToArray();
        return collectedChars;
    }
    /*
    public void SetSkill(BasicAttack attack, eCHARTYPE type)
    {
        _skillSetter.SetSkill(attack, type);
        
        
    }
    */
}
