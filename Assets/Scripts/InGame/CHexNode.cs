using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexNode
{
    public int _XPos { get; private set; }
    public int _YPos { get; private set; }
    public HexNode _TopLeft { get; set; }
    public HexNode _TopRight { get; set; }
    public HexNode _Left { get; set; }
    public HexNode _Right { get; set; }
    public HexNode _BottomLeft { get; set; }
    public HexNode _BottomRight { get; set; }
    public eTILESTATE _TileState { get; private set; }

    public Vector3 _TilePos { get; private set; }
    public GameObject _charGo { get; private set; }
    public HexNode(int row, int col)
    {
        _XPos = row;
        _YPos = col;
    }
    public void SetTileState(eTILESTATE TileState)
    {
        _TileState = TileState;
    }
    public void SetTilePos(Vector3 tilePos)
    {
        _TilePos = tilePos;
    }
    public void SetCharGo(GameObject go)
    {
        _charGo = go;
    }
}
