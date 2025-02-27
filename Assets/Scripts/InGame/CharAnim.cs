using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim : MonoBehaviour
{
    [Header("공격"), SerializeField] GameObject _attackGo;
    IBasicAttack _basicAttack;

    public GameObject _targetObj = null;

    Animator _anim;

    float _attSpeed = 0f;

    CharMove _charMove;
    private void Awake()
    {
        if (_attackGo != null)
        {
            _basicAttack = _attackGo.GetComponent<IBasicAttack>();
        }
        _anim = GetComponent<Animator>();
    }
    public void SetCharMove(CharMove charMove)
    {
        _charMove = charMove;
    }
    public void StartAttack(float attSpeed, GameObject target)
    {
        _attSpeed = attSpeed;

        //Debug.Log("공격 동작 시작");
        _targetObj = target;
        _charMove.SetRotation(_targetObj.transform.position);
        _basicAttack.SetTarget(_targetObj);


        
        _anim.SetTrigger("PlayAttack");
        //StartCoroutine(CRT_Attack());
    }
    public void StopAttack()
    {
        _anim.SetTrigger("StopAttack");
    }
    public void SetIsWalking(bool isWalking)
    { 
        _anim.SetBool("IsWalking", isWalking); 
    }
    public void Anim_Hit(int isRanged)
    {
        _basicAttack.StartAttack(isRanged == 1);
    }
    public void Anim_FinishAttack()
    {
        _charMove.FinishAttack();
    }
}
