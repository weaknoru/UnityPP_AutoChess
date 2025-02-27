using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_KaiSa : MonoBehaviour, ISkill
{
    [Header("파티클"), SerializeField] ParticleSystem _particle;

    [Header("점프 속도"), SerializeField] float _jumpSpeed = 15f;
    [Header("레벨 별 공격속도"), SerializeField] float[] _attSpeedList = new float[3] { 0.5f, 0.75f, 1f };
    [Header("레벨 별 보호막"), SerializeField] float[] _shieldList = new float[3] { 400f, 700f, 1000f };

    int _level = 0;
    float _apIncrease = 0f;

    eTILESTATE _state;

    GameObject _targetObj = null;

    CharInfo _charInfo;
    private void Awake()
    {
        _particle.Stop(true);
    }
    public void ExecuteSkill()
    {

        if (_particle != null)
        {
            _particle.Play(true);
        }
        _charInfo.JumpToFarthestChar(2, _jumpSpeed);
        StartCoroutine(CRT_Skill());
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
