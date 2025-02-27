using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharHealth : MonoBehaviour
{
    [Header("최대 체력"), SerializeField] float _maxHp = 10;
    [Header("현재 체력"), SerializeField] float _curHp = 0f;
    [Header("타일 컨트롤러"), SerializeField] CharTile _charTile;

    [Header("방어력"), SerializeField] float _def = 10;
    [Header("마법저항력"), SerializeField] float _mr = 0f;

    Slider _hpSlider;

    GameObject _tmpObj;
    GameObject _hpBarGo;

    bool _isPlayer = true;

    bool _isDead = false;
    private void Awake()
    {
        if (_charTile == null)
        {
            _charTile = GetComponent<CharTile>();
        }
        _tmpObj = gameObject;
    }
    public void SetIsPlayer(bool isPlayer)
    {
        _isPlayer = isPlayer;
    }
    public void LoseAdDamage(float damage)
    {
        float finalDamage = damage * (100 / (100 + _def));
        _curHp -= finalDamage;
        UpdateHpBar(_curHp);
        if (_curHp <= 0f && !_isDead)
        {
            Dead();
        }
    }
    public void GetHeal(float amount)
    {
        _curHp += amount;
        if(_curHp > _maxHp)
        {
            _curHp = _maxHp;
        }
        UpdateHpBar(_curHp);
    }
    public void LoseApDamage(float damage)
    {
        float finalDamage = damage * (100 / (100 + _mr));
        _curHp -= finalDamage;
        UpdateHpBar(_curHp);
        if (_curHp <= 0f && !_isDead)
        {
            Dead();
        }
    }
    public void SetHpBar(MoveBar hpSlider)
    {
        _hpBarGo = hpSlider.gameObject;
        _hpSlider = hpSlider._HpBar.GetComponent<Slider>();
        
    }
    void UpdateHpBar(float value)
    {
        _hpSlider.value = value;
        

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
