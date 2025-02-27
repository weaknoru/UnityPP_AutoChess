using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCC
{
    Stun,
    Airbone,
    Bind
}
public class CCController : MonoBehaviour
{
    [SerializeField] CharMove _charMove;


    private void Awake()
    {
        if(_charMove == null)
        {
            _charMove = GetComponent<CharMove>();

        }
    }
    public void SetMoveCC(eCC cc, float dur)
    {
        switch(cc)
        {
            case eCC.Stun:
                _charMove.SetStun(dur);
                break;
            case eCC.Airbone:
                _charMove.SetAirbone(dur);
                break;
            case eCC.Bind:
                _charMove.SetBind(dur);
                break;
        }
    }
}
