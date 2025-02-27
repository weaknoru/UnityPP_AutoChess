using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueInfo : Tile
{
    [Header("대기열 인덱스"), SerializeField] float _queueIndex;
    [Header("챔피언 배치 위치"), SerializeField] Transform _charTrs;

    [Header("플레이어 여부"), SerializeField] bool _isPlayerQueue = false;
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
