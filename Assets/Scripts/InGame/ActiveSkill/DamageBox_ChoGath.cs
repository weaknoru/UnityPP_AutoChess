using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox_ChoGath : MonoBehaviour
{
    Collider _col;
    eTILESTATE _targetState;

    float _damage = 0f;
    float _airBoneDur = 0f;
    private void Awake()
    {
        _col = GetComponent<Collider>();
        _col.enabled = false;
    }
    public void Interact(float damage, float airBoneDur)
    {
        _damage = damage;
        _airBoneDur = airBoneDur;
        StartCoroutine(CRT_ActiveDamageBox());
    }
    public void SetTargetState(eTILESTATE target)
    {
        _targetState = target;
    }
    IEnumerator CRT_ActiveDamageBox()
    {
        _col.enabled = true;
        yield return new WaitForSeconds(0.05f);
        _col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        CharInfo charInfo = other.GetComponent<CharInfo>(); 
        if(charInfo._State == _targetState)
        {
            CCController ccController = other.GetComponent<CCController>();
            ccController.SetMoveCC(eCC.Airbone, _airBoneDur);

            CharHealth health = other.GetComponent<CharHealth>();
            health.LoseApDamage(_damage);
        }
    }
}
