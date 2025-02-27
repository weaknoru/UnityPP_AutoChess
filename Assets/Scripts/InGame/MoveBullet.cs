using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    private float _speed = 30f;
    private float _damage = 0f;
    private GameObject _targetObj;

    BasicAttack _basicAttack;

    public void ShootBullet(GameObject targetObj, float damage)
    {
        _targetObj = targetObj;
        _damage = damage;
        StartCoroutine(CRT_MoveBullet());
    }

    IEnumerator CRT_MoveBullet()
    {
        while (Vector3.Distance(transform.position, _targetObj.transform.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetObj.transform.position, _speed * Time.deltaTime);
            yield return null;
        }

        // 총알이 목표물에 도달하면 데미지 적용 후 비활성화
        CharHealth health = _targetObj.GetComponent<CharHealth>();
        health.LoseAdDamage(_damage);
        _basicAttack.AddMana();
        gameObject.SetActive(false);
    }
    public void SetBasicAttack(BasicAttack basicAttack)
    {
        _basicAttack = basicAttack;
    }
}
