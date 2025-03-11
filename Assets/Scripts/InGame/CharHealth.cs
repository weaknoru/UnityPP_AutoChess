using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharHealth : MonoBehaviour
{
    [Header("�ִ� ü��"), SerializeField] float _maxHp = 10;
    [Header("���� ü��"), SerializeField] float _curHp = 0f;
    public float _CurHp => _curHp;
    [Header("���� ����"), SerializeField] float _curShield = 0f;
    [Header("Ÿ�� ��Ʈ�ѷ�"), SerializeField] CharTile _charTile;
    [Header("����"), SerializeField] float _def = 10;
    [Header("�������׷�"), SerializeField] float _mr = 0f;

    Slider _hpSlider;
    Slider _shieldSlider;
    GameObject _tmpObj;
    GameObject _hpBarGo;
    bool _isPlayer = true;
    bool _isDead = false;

    private class ShieldData
    {
        public float Value { get; set; }
        public float ExpirationTime { get; set; }
    }

    private List<ShieldData> _shieldList = new List<ShieldData>();

    private void Awake()
    {
        if (_charTile == null)
        {
            _charTile = GetComponent<CharTile>();
        }
        _tmpObj = gameObject;
        _curHp = _maxHp;
    }

    public void SetIsPlayer(bool isPlayer)
    {
        _isPlayer = isPlayer;
    }

    // AD ������ ó�� (���� ������)
    public void LoseAdDamage(float damage)
    {
        float finalDamage = damage;

        // ���� ���� ó��
        while (finalDamage > 0 && _shieldList.Count > 0)
        {
            ShieldData currentShield = _shieldList[0];

            if (finalDamage >= currentShield.Value)
            {
                finalDamage -= currentShield.Value;
                _shieldList.RemoveAt(0);
            }
            else
            {
                currentShield.Value -= finalDamage;
                finalDamage = 0;
            }

            UpdateShieldBar(GetTotalShield());
        }

        // ���� ����
        if (finalDamage > 0)
        {
            float reducedDamage = finalDamage * (100 / (100 + _def));
            _curHp -= reducedDamage;
            UpdateHpBar(_curHp);
        }

        CheckDeath();
    }

    // AP ������ ó�� (���� ������)
    public void LoseApDamage(float damage)
    {
        float finalDamage = damage;

        // ���� ���� ó��
        while (finalDamage > 0 && _shieldList.Count > 0)
        {
            ShieldData currentShield = _shieldList[0];

            if (finalDamage >= currentShield.Value)
            {
                finalDamage -= currentShield.Value;
                _shieldList.RemoveAt(0);
            }
            else
            {
                currentShield.Value -= finalDamage;
                finalDamage = 0;
            }

            UpdateShieldBar(GetTotalShield());
        }

        // ���� ���׷� ����
        if (finalDamage > 0)
        {
            float reducedDamage = finalDamage * (100 / (100 + _mr));
            _curHp -= reducedDamage;
            UpdateHpBar(_curHp);
        }

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_curHp <= 0f && !_isDead)
        {
            Dead();
        }
    }

    // ���� �߰� �޼���
    public void SetShield(float value, float duration)
    {
        _shieldSlider.maxValue = _maxHp;
        ShieldData newShield = new ShieldData
        {
            Value = value,
            ExpirationTime = Time.time + duration
        };

        _shieldList.Add(newShield);
        UpdateShieldBar(GetTotalShield());

        StartCoroutine(ShieldExpirationRoutine(newShield, duration));
    }

    // ���� ���� �ڷ�ƾ
    private IEnumerator ShieldExpirationRoutine(ShieldData shield, float duration)
    {
        yield return new WaitForSeconds(duration);

        _shieldList.Remove(shield);
        UpdateShieldBar(GetTotalShield());
    }

    // �� ���� ��� �޼���
    private float GetTotalShield()
    {
        float totalShield = 0;
        foreach (var shield in _shieldList)
        {
            totalShield += shield.Value;
        }
        return totalShield;
    }

    public void GetHeal(float amount)
    {
        _curHp += amount;
        _curHp = Mathf.Min(_curHp, _maxHp);
        UpdateHpBar(_curHp);
    }

    public void SetHpBar(MoveBar hpSlider)
    {
        _hpBarGo = hpSlider.gameObject;
        _hpSlider = hpSlider._HpBar.GetComponent<Slider>();
        _shieldSlider = hpSlider._ShieldBar.GetComponent<Slider>();

        _hpSlider.maxValue = _maxHp;
        _shieldSlider.maxValue = _maxHp;
    }

    void UpdateHpBar(float value)
    {
        _hpSlider.value = value;
    }

    void UpdateShieldBar(float value)
    {
        _shieldSlider.value = value;
    }

    public void SetHealthStat(float maxHp, float def, float mr)
    {
        _maxHp = maxHp;
        _def = def;
        _mr = mr;
        _curHp = _maxHp;
        _hpSlider.maxValue = _maxHp;
        _hpSlider.value = _maxHp;
    }

    void Dead()
    {
        _isDead = true;
        StageManager.Instance.MinusCount(_isPlayer);
        _charTile.Dead();
        Destroy(_hpBarGo);
        Destroy(_tmpObj);
    }
}