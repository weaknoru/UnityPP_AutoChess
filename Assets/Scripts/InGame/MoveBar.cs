using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MoveBar : MonoBehaviour
{
    Camera _mainCam;
    Canvas _canvas;

    [Header("HP��"), SerializeField] GameObject _hpBar;
    public GameObject _HpBar => _hpBar;
    [Header("MP��"), SerializeField] GameObject _mpBar;
    public GameObject _MpBar => _mpBar;
    [Header("���� ��"), SerializeField] GameObject _shieldBar;
    public GameObject _ShieldBar => _shieldBar;
    [Header("���� ������"), SerializeField] GameObject[] _levelIcons;

    [Header("�Ʊ� / �� hp ����"), SerializeField] Color[] _hpBarColors;
    [SerializeField] Image _hpBarImg;
    private void Awake()
    {
        foreach(GameObject icon in _levelIcons)
        {
            icon.SetActive(false);
        }
    }
    public void SetCanvasTrs(Canvas canvas)
    {
        _canvas = canvas;
    }
    public void MoveBarObj(Vector3 charPos)
    {
        _mainCam = Camera.main;

        Vector3 screenPosition = _mainCam.WorldToScreenPoint(charPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                screenPosition,
                _canvas.worldCamera,
                out Vector2 localPosition
            );

        // HP ���� ��ġ�� ������Ʈ
        transform.localPosition = localPosition;
    }
    public void SetAwakePos(Vector3 camPos)
    {
        MoveBarObj(camPos);
    }
    public void SetLevelIcon(int level)
    {
        _levelIcons[level - 1].SetActive(true);
    }
    public void SetHpBarColor(eTILESTATE state)
    {
        Color barColor = (state == eTILESTATE.Player) ? _hpBarColors[0] : _hpBarColors[1];
        _hpBarImg.color = barColor;
    }
}
