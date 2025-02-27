using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RekSai : MonoBehaviour, ISkill
{
    [Header("회복 파티클"), SerializeField] ParticleSystem _healParticle;
    [Header("에어본 파티클"), SerializeField] ParticleSystem _airboneParticle;

    [Header("에어본 지속시간"), SerializeField] float _airboneDur = 1.75f;
    [Header("힐 시간"), SerializeField] float _healDur = 1f;

    [Header("레벨 별 힐량"), SerializeField] float[] _healList = new float[3] { 150f, 250f, 350f };
    [Header("레벨 별 데미지"), SerializeField] float[] _damageList = new float[3] { 400f, 600f, 800f };

    int _level = 0;
    float _apIncrease = 0f;

    eTILESTATE _state;

    GameObject _targetObj = null;

    CharInfo _charInfo;
    private void Awake()
    {
        _healParticle.Stop();
        _airboneParticle.Stop();
    }
    public void ExecuteSkill()
    {

        _healParticle.Play(true);
        StartCoroutine(CRT_Skill());
        /*
        if (_targetObj != null)
        {
            GiveDamage(_targetObj.transform);
        }
        */
        
    }
    IEnumerator CRT_Skill()
    {

        int count = 5;
        float healPerCount = _healList[_level - 1] / count;

        float durPerCount = _healDur / count;
        for (int i = 0; i < count; i++)
        {
            _charInfo.GetHeal(healPerCount);
            yield return new WaitForSeconds(durPerCount);
        }
        _healParticle.Stop();
        Vector3 targetPos = _targetObj.transform.position;
        _airboneParticle.transform.position = targetPos;
        _airboneParticle.Play();
        
        GiveDamage(_targetObj.transform);
        
        _charInfo.FinishSkill();
    }
    void GiveDamage(Transform trs)
    {
        CharTile charTile = _targetObj.GetComponent<CharTile>();
        Vector2 pos = charTile._tilePos;
        eTILESTATE targetState = (_state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
        

        float damage = _damageList[_level - 1];
        //Debug.Log(damage);

        CharHealth health = trs.GetComponent<CharHealth>();
        health.LoseApDamage(damage + (damage * _apIncrease * 0.01f));


        CCController ccController = trs.GetComponent<CCController>();
        ccController.SetMoveCC(eCC.Airbone, _airboneDur);
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
