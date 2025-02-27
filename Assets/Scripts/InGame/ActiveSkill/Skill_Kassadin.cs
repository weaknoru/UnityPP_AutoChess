using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Kassadin : MonoBehaviour, ISkill
{

    [Header("��ƼŬ"), SerializeField] ParticleSystem[] _particles;

    public List<Transform> _nearEnemies = new List<Transform>();

    [Header("�߻� ��ġ"), SerializeField] Transform _shootTrs;
    [Header("�� Layer"), SerializeField] LayerMask _enemyLayer;

    [Header("���� �� ������"), SerializeField] float[] _damageList = new float[3] {250f, 500f, 800f};
    [Header("���� �� ���� �ð�"), SerializeField] float[] _durList = new float[3] { 2.5f, 3f, 3.5f};

    int _level = 0;
    float _apIncrease = 0f;
    [Header("�߻� ��ġ"), SerializeField] float _distance = 10f;
    [Header("����"), SerializeField] float _angle;

    eTILESTATE _state;

    GameObject _targetObj = null;

    CharInfo _charInfo;
    public void ExecuteSkill()
    {
        //Debug.Log("FSkill_Kassadin Activated!");
        
        if(_particles != null)
        {
            _particles[0].Play(true);
        }
        GetEnemiesInFanShape();

        if(_nearEnemies.Count > 0)
        {
            foreach (Transform t in _nearEnemies)
            {
                eTILESTATE targetState = (_state == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
                CharInfo info = t.GetComponent<CharInfo>();
                if(info != null)
                {
                    if (info._State == targetState)
                    {
                        GiveDamage(t);
                    }
                }
            }
        }
        
        StartCoroutine(CRT_Skill());

    }
    IEnumerator CRT_Skill()
    {
        yield return new WaitForSeconds(1f);
        _charInfo.FinishSkill();
    }
    public void ClearNearEnemies()
    {
        _nearEnemies.Clear();
        //_nearEnemies.RemoveAll(target => target == null);
    }
    void GiveDamage(Transform trs)
    {
        float damage = _damageList[_level - 1];

        CharHealth health = trs.GetComponent<CharHealth>();
        health.LoseApDamage(damage + (damage * _apIncrease * 0.01f));


        float dur = _durList[_level - 1];
        CCController ccController = trs.GetComponent<CCController>();
        ccController.SetMoveCC(eCC.Stun, dur);
    }
    void GetEnemiesInFanShape()
    {
        ClearNearEnemies();

        
        Collider[] hits = Physics.OverlapSphere(_shootTrs.position, _distance, _enemyLayer);

        foreach (Collider hit in hits)
        {
            if (!_nearEnemies.Contains(hit.transform))
            {
                Vector3 directionToEnemy = (hit.transform.position - _shootTrs.position).normalized;
                float angleToEnemy = Vector3.Angle(_shootTrs.forward, directionToEnemy);

                if (angleToEnemy <= _angle)
                {
                    _nearEnemies.Add(hit.transform);
                }
            }
        }

    }

    public void SetBuff(int level, float apIncrease)
    {
        _level = level;
        _apIncrease = apIncrease;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // ��ä���� ���۰� �� ���� ���
        Vector3 leftBoundary = Quaternion.Euler(0, -_angle, 0) * _shootTrs.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, _angle, 0) * _shootTrs.forward;

        // ��ä���� ���� �� �׸���
        Gizmos.DrawLine(_shootTrs.position, _shootTrs.position + leftBoundary * _distance);
        Gizmos.DrawLine(_shootTrs.position, _shootTrs.position + rightBoundary * _distance);
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
