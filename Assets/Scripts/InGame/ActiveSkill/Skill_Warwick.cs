using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Warwick : MonoBehaviour, ISkill
{
    [Header("파티클"), SerializeField] ParticleSystem[] _particles;


    [Header("레벨 별 데미지"), SerializeField] float[] _damageList = new float[3] { 150f, 225f, 300f };
    [Header("기절 시간"), SerializeField] float _stunDur = 1.5f;

    int _level = 0;
    float _apIncrease = 0f;

    eTILESTATE _state;

    GameObject _targetObj = null;

    CharInfo _charInfo;

    Transform _weakestTrs;
    private void Awake()
    {
        if (_particles[0] != null)
        {
            _particles[0].Stop();
            _particles[0].transform.SetParent(null);
        }
        if (_particles[1] != null)
        {
            _particles[1].Stop();
        }
    }
    public void ExecuteSkill()
    {
        _weakestTrs = GetWeakestUnit();

        if (_particles[0] != null)
        {
            _particles[0].transform.position = _weakestTrs.transform.position;
            _particles[0].Play(true);
        }
        if (_particles[1] != null)
        {
            _particles[1].Play(true);
        }


        StartCoroutine(CRT_Skill());
    }
    IEnumerator CRT_Skill()
    {
        int count = 3;
        CharMove charMove = _weakestTrs.GetComponent<CharMove>();
        charMove.SetStun(_stunDur);
        float durPerCount = _stunDur / count;
        for (int i = 0; i < count; i++)
        {
            GiveDamage(_weakestTrs);
            yield return new WaitForSeconds(durPerCount);
        }
        _particles[0].Stop(true);
        _particles[1].Stop(true);
        _charInfo.FinishSkill();
    }
    Transform GetWeakestUnit()
    {
        List<Transform> targetList = (_state == eTILESTATE.Player) ? CharManager.Instance._EnemyTrses : CharManager.Instance._PlayerTrses;
        Transform weakestUnit = targetList[0];
        

        CharHealth health = targetList[0].GetComponent<CharHealth>();
        float lowestHp = health._CurHp;
        foreach (Transform trs in targetList)
        {
            health = trs.GetComponent<CharHealth>();
            if(lowestHp > health._CurHp)
            {
                weakestUnit = trs;
            }
        }
        return weakestUnit;
    }
    void GiveDamage(Transform trs)
    {
        CharTile charTile = _targetObj.GetComponent<CharTile>();
        //Vector2 pos = charTile._tilePos;
        int count = 3;
        float damagePerCount = _damageList[_level - 1] / count;
        //Debug.Log(damage);

        CharHealth health = trs.GetComponent<CharHealth>();
        health.LoseApDamage(damagePerCount + (damagePerCount * _apIncrease * 0.01f));

        
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
