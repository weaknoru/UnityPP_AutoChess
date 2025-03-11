using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lulu : MonoBehaviour, ISkill
{
    [Header("파티클"), SerializeField] ParticleSystem[] _particles;


    [Header("레벨 별 데미지"), SerializeField] float[] _damageList = new float[3] { 150f, 250f, 350f };
    [Header("레벨 별 고립 데미지"), SerializeField] float[] _soloDamageList = new float[3] { 400f, 600f, 800f };

    int _level = 0;
    float _apIncrease = 0f;

    eTILESTATE _state;

    GameObject _targetObj = null;

    CharInfo _charInfo;
    private void Awake()
    {
        _particles[0].Stop();
    }
    public void ExecuteSkill()
    {

        if (_particles != null)
        {
            _particles[0].Play(true);
        }

        if (_targetObj != null)
        {
            GiveDamage(_targetObj.transform);
        }

        StartCoroutine(CRT_Skill());
    }
    IEnumerator CRT_Skill()
    {
        yield return new WaitForSeconds(1f);
        _charInfo.FinishSkill();
    }
    void GiveDamage(Transform trs)
    {
        CharTile charTile = _targetObj.GetComponent<CharTile>();
        Vector2 pos = charTile._tilePos;
        eTILESTATE targetState = (_state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
        bool hasNaighbor = HexBoardMaker.Instance._hexGrid.CheckNeighbors((int)pos.x, (int)pos.y, targetState);

        float damage = hasNaighbor ? _damageList[_level - 1] : _soloDamageList[_level - 1];
        Debug.Log(damage);

        CharHealth health = trs.GetComponent<CharHealth>();
        health.LoseApDamage(damage + (damage * _apIncrease * 0.01f));

    }


    public void SetBuff(int level, float apIncrease)
    {
        _level = level;
        _apIncrease = apIncrease;
    }



    public void SetTileState(eTILESTATE tileType)
    {
        _state = tileType;
    }

    public void SetTargetObj(GameObject targetObj)
    {
        _targetObj = targetObj;


    }

    public void SetCharInfo(CharInfo charInfo)
    {
        _charInfo = charInfo;
    }
}
