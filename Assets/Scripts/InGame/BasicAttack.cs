using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public interface IBasicAttack
{
    void StartAttack(bool isRanged);
    void SetTarget(GameObject target);
    void SetAttDamage(float damage);
}

public class BasicAttack : MonoBehaviour, IBasicAttack
{
    [Header("�Ѿ� ������"), SerializeField] GameObject _bulletPref;
    [Header("�߻� ��ġ"), SerializeField] Transform _shootTrs;
    [Header("������Ʈ Ǯ ũ��"), SerializeField] int _poolSize = 5;

    private List<GameObject> _bulletPool = new List<GameObject>();
    private GameObject _targetObj = null;
    [Header("������"), SerializeField] float _damage = 1f;

    //[Header("��ų ����"), SerializeField] SkillAttack _skillAttack;
    [Header("��ų ������Ʈ"), SerializeField] GameObject _skillObj;
    ISkill _skill;

    eTILESTATE _tileState;

    GameObject _mpBarGo = null;
    Slider _mpSlider;

    //[Header("��ƼŬ"), SerializeField] ParticleSystem[] _particles;
    float _mpRecovery = 0f;

    float _maxMp = 0f;
    float _curMp = 0f;

    int _level;
    float _apIncrease = 0f;

    CharInfo _charInfo;

    private void Awake()
    {
        if(_bulletPref != null)
        {
            // ������Ʈ Ǯ �ʱ�ȭ
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject bullet = Instantiate(_bulletPref, _shootTrs.position, Quaternion.identity);
                bullet.SetActive(false);
                _bulletPool.Add(bullet);

                MoveBullet moveBullet = bullet.GetComponent<MoveBullet>();
                moveBullet.SetBasicAttack(this);
            }
        }
        
        
    }

    public void StartAttack(bool isRanged)
    {
        if (_targetObj != null)
        {
            if (isRanged) // ���Ÿ�
            {
                ShootBullet();
            }
            else // �ٰŸ�
            {
                StartSlash();
            }
        }
        else
        {
            Debug.Log(transform.parent.name + "���� Ÿ�� ����");
        }
    }
    /*
    public void SetActiveSKill(ISkill iSkill)
    {
        _skillAttack.SetSkill(iSkill);
    }
    */
    public void SetTarget(GameObject target)
    {
        _targetObj = target;
        if (_skill != null)
        {
            _skill.SetTargetObj(_targetObj);
        }
    }
    public void SetLevel(int level)
    {
        _level = level;
        
    }
    public void SetTileState(eTILESTATE tileState)
    {
        _tileState = tileState;
        if (_skill != null)
        {
            _skill.SetTileState(_tileState);
        }
    }

    void StartSlash()
    {
        //Debug.Log("������ ����");
        CharHealth health = _targetObj.GetComponent<CharHealth>();
        health.LoseAdDamage(_damage);
        AddMana();
    }

    void ShootBullet()
    {
        //Debug.Log("�Ѿ� �߻� ����");

        // ��Ȱ��ȭ�� �Ѿ� ã��
        GameObject bullet = GetPooledBullet();
        if (bullet != null)
        {
            bullet.transform.position = _shootTrs.position;
            bullet.SetActive(true);
            MoveBullet moveBullet = bullet.GetComponent<MoveBullet>();
            moveBullet.ShootBullet(_targetObj, _damage);
        }
        else
        {
            Debug.Log("��� ������ �Ѿ��� �����ϴ�.");
        }
    }
    public void SetManaRecover(float maxMp, float startMp, float mpRecovery)
    {
        _maxMp = maxMp;
        _curMp = startMp;
        _mpRecovery = mpRecovery;
    }
    public void AddMana()
    {
        _curMp += _mpRecovery;
        if(_curMp > _maxMp)
        {
            if(_skill != null)
            {
                _skill.SetBuff(_level, _apIncrease);
                
                _charInfo.SetManaState();
            }
            
            _curMp = 0f;
        }
        UpdateMpSlider(_curMp);

    }
    public void ExecuteSkill()
    {
        _skill.ExecuteSkill();
        
    }
    public void AddMana(float damage)
    {
        
    }
    public void SetAttDamage(float damage)
    {
        _damage = damage;
    }
    public void SetApBuff(float apIncrease)
    {
        _apIncrease = apIncrease;
    }
    public void SetMpBar(GameObject mpSlider)
    {
        _mpBarGo = mpSlider;
        _mpSlider = _mpBarGo.GetComponent<Slider>();
        _mpSlider.maxValue = _maxMp;
        _mpSlider.value = _curMp;
    }
    void UpdateMpSlider(float value)
    {
        _mpSlider.value = value;
    }
    // ������Ʈ Ǯ���� ��Ȱ��ȭ�� �Ѿ� ��������
    GameObject GetPooledBullet()
    {
        foreach (GameObject bullet in _bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }

    public void SetCharInfo(CharInfo info)
    {
        if (_skillObj == null)
        {
            _skillObj = gameObject;
            _skill = _skillObj.GetComponent<ISkill>();
            _charInfo = info;
            _skill.SetCharInfo(_charInfo);
        }
        else
        {
            Debug.Log("_skillObj Null");
        }
        
    }
}