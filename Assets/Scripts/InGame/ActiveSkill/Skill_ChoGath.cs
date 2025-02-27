using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ChoGath : MonoBehaviour, ISkill
{

    [Header("에어본 파티클"), SerializeField] ParticleSystem _airboneParticle;

    [Header("에어본 반지름"), SerializeField] float _radius = 6f;

    [Header("데미지 박스"), SerializeField] GameObject _damageBoxObj;
    DamageBox_ChoGath _damageBox;
    [Header("레벨 별 에어본 지속시간"), SerializeField] float[] _airboneDurList = new float[3] { 1.5f, 1.75f, 2f };
    [Header("레벨 별 데미지"), SerializeField] float[] _damageList = new float[3] { 200f, 400f, 600f };

    int _level = 0;
    float _apIncrease = 0f;

    eTILESTATE _state;

    GameObject _targetObj = null;

    CharInfo _charInfo;

    private void Awake()
    {
        _damageBox = _damageBoxObj.GetComponent<DamageBox_ChoGath>();

        _airboneParticle.Stop();
        _damageBoxObj.transform.localScale = Vector3.one * _radius;
    }
    public void ExecuteSkill()
    {

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

        Vector3 targetPos = _targetObj.transform.position;
        _damageBoxObj.transform.position = targetPos;

        _airboneParticle.Play(true);
        yield return new WaitForSeconds(1f);
        GiveDamage(_targetObj.transform);

        _charInfo.FinishSkill();
    }
    void GiveDamage(Transform trs)
    {
        float damage = _damageList[_level - 1] + (_damageList[_level - 1] * _apIncrease * 0.01f);
        
        float airBoneDur = _airboneDurList[_level - 1];
        _damageBox.Interact(damage, airBoneDur);
        
        

        
        //Debug.Log(damage);

        


        
    }


    public void SetBuff(int level, float apIncrease)
    {
        _level = level;
        _apIncrease = apIncrease;
    }



    public void SetTileState(eTILESTATE tileType)
    {
        _state = tileType;
        eTILESTATE target = (_state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
        _damageBox.SetTargetState(target);
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
