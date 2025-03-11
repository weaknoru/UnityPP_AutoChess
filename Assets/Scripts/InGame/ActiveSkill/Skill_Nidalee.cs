using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Nidalee : MonoBehaviour, ISkill
{
    [Header("파티클"), SerializeField] ParticleSystem[] _particles;


    [Header("레벨 별 데미지"), SerializeField] float[] _damageList = new float[3] { 20f, 70f, 120f };

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

        //_charInfo.IncreaseStat(eSTAT.Range, -2);
        StartCoroutine(CRT_Skill());
        float increaseAd = _damageList[_level - 1];
        _charInfo.IncreaseStat(eSTAT.Ad, increaseAd);
        
    }
    IEnumerator CRT_Skill()
    {
        yield return new WaitForSeconds(1f);
        _charInfo.FinishSkill();
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
