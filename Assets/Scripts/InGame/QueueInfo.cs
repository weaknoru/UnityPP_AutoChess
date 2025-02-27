using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueInfo : Tile
{
    [Header("��⿭ �ε���"), SerializeField] float _queueIndex;
    [Header("è�Ǿ� ��ġ ��ġ"), SerializeField] Transform _charTrs;

    [Header("�÷��̾� ����"), SerializeField] bool _isPlayerQueue = false;
    public bool _IsPlayerQueue => _isPlayerQueue;

    public void SetIsPlayer(bool isPlayerQueue)
    {
        _isPlayerQueue = isPlayerQueue;
    }
    public override void PutCharacter(GameObject charObj)
    {
        charObj.transform.position = _charTrs.position;
        charObj.transform.rotation = _charTrs.rotation;
        MovableUnit movable = charObj.GetComponent<MovableUnit>();
        movable.ResetHpBarPos();
    }

    public override void SetTileState(eTILESTATE state)
    {
        _state = state;
    }

    public override void SetCharObj(GameObject charObj)
    {
        
    }
}
