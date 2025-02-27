using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public HexNode _hexNode;
    public eTILESTATE _state;
    public GameObject _onCharGo;
    public abstract void PutCharacter(GameObject charObj);
    public abstract void SetTileState(eTILESTATE state);
    public abstract void SetCharObj(GameObject charObj);
}
