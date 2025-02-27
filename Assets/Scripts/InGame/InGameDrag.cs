using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameDrag : MonoBehaviour
{
    static InGameDrag _instance;

    public static InGameDrag Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<InGameDrag>();
            return _instance;
        }
    }

    

    Vector3 _pos = Vector3.zero;
    public GameObject _clickedObj = null;
    MoveBar _moveHpBar = null;


    public void SetClickedObj(GameObject obj)
    {
        _clickedObj = obj;
    }
    public void SetClickedHpBar(MoveBar moveHpBar)
    {
        _moveHpBar = moveHpBar;
    }
    /*
    void OnMouseDrag()
    {
        if(!_isClicked)
        {
            return;
        }
        Vector3 worldPosition = Vector3.zero;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
        {
            _pos = hit.point;
        }
        _pos.y = 1f;
        transform.position = _pos;
    }
    */
    public void StartDrag()
    {
        StartCoroutine(CRT_StartDrag());
    }
    IEnumerator CRT_StartDrag()
    {
        while (_clickedObj != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 3))
            {
                _pos = hit.point;
            }
            _pos.y = 1f;
            _clickedObj.transform.position = _pos;
            _moveHpBar.MoveBarObj(_pos);
            yield return null;
            /*
            if(Input.GetMouseButtonUp(0))
            {
                break;
            }
            */
        }
    }
}
