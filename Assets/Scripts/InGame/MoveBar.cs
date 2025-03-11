using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
    private void Awake()
    {
        
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
}
