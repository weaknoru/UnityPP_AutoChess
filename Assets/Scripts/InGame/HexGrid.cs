using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    private int _xs;
    private int _ys;
    private HexNode[,] _grid;

    public int _xSize { get; private set; }
    public int _ySize { get; private set; }
    public HexGrid(int xs, int ys)
    {
        this._xs = xs;
        this._ys = ys;
        _grid = new HexNode[xs, ys];

        // 노드 생성
        for (int x = 0; x < xs; x++)
        {
            for (int y = 0; y < ys; y++)
            {
                _grid[x, y] = new HexNode(x, y);
            }
        }

        // 인접 노드 연결
        for (int x = 0; x < xs; x++)
        {
            for (int y = 0; y < ys; y++)
            {
                ConnectNeighbors(x, y);
            }
        }

        _xSize = xs;
        _ySize = ys;
    }

    private void ConnectNeighbors(int x, int y) //8,7
    {
        bool isOffsetRow = y % 2 != 0; //짝수열이면 false
        _grid[x, y]._TopLeft = (y < _ys - 1 && (!isOffsetRow || x > 0)) ? _grid[x - (isOffsetRow ? 1 : 0), y + 1] : null;
        _grid[x, y]._TopRight = (y < _ys - 1 && (isOffsetRow || x < _xs - 1)) ? _grid[x + (isOffsetRow ? 0 : 1), y + 1] : null;
        _grid[x, y]._Left = (x > 0) ? _grid[x - 1, y] : null;
        _grid[x, y]._Right = (x < _xs - 1) ? _grid[x + 1, y] : null;
        _grid[x, y]._BottomLeft = (y > 0 && (!isOffsetRow || x > 0)) ? _grid[x - (isOffsetRow ? 1 : 0), y - 1] : null;
        _grid[x, y]._BottomRight = (y > 0 && (isOffsetRow || x < _xs - 1)) ? _grid[x + (isOffsetRow ? 0 : 1), y - 1] : null;
        
    }
    public bool CheckNeighbors(int x, int y, eTILESTATE targetState)
    {
        if (_grid[x, y]._TopLeft != null &&
        _grid[x, y]._TopLeft._TileState == targetState &&
        _grid[x, y]._TopLeft._charGo != null)
            return true;

        if (_grid[x, y]._TopRight != null &&
            _grid[x, y]._TopRight._TileState == targetState &&
            _grid[x, y]._TopRight._charGo != null)
            return true;

        if (_grid[x, y]._Left != null &&
            _grid[x, y]._Left._TileState == targetState &&
            _grid[x, y]._Left._charGo != null)
            return true;

        if (_grid[x, y]._Right != null &&
            _grid[x, y]._Right._TileState == targetState &&
            _grid[x, y]._Right._charGo != null)
            return true;

        if (_grid[x, y]._BottomLeft != null &&
            _grid[x, y]._BottomLeft._TileState == targetState &&
            _grid[x, y]._BottomLeft._charGo != null)
            return true;

        if (_grid[x, y]._BottomRight != null &&
            _grid[x, y]._BottomRight._TileState == targetState &&
            _grid[x, y]._BottomRight._charGo != null)
            return true;

        return false;
    }
    public HexNode FindNodeByGameObject(GameObject targetGo)
    {
        for(int i = 0; i < _xs; i++)
        {
            for(int j = 0; j < _ys; j++)
            {
                if(GetNode(i, j)._charGo == targetGo)
                {
                    return GetNode(i, j);
                }
            }
        }
        
        return null; // targetGo를 가진 노드가 없으면 null 반환
    }
    public HexNode GetNode(int x, int y)
    {
        return (x >= 0 && x < _xs && y >= 0 && y < _ys) ? _grid[x, y] : null;
    }
    public HexNode GetAwayNode(HexNode startNode, bool targetIsRight, int range)
    {
        int count = targetIsRight ? 1 : -1;
        HexNode awayNode = null;
        if(targetIsRight)
        {
            for (int i = 0; i < range; i++)
            {
                if (startNode._Right != null && startNode._Right._TileState == eTILESTATE.Empty)
                {
                    awayNode = startNode._Right;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                if (startNode._Left != null && startNode._Left._TileState == eTILESTATE.Empty)
                {
                    awayNode = startNode._Left;
                }
                else
                {
                    break;
                }
            }

        }
        if(awayNode == null)
        {
            if(targetIsRight)
            {
                if (startNode._TopRight != null && startNode._TopRight._TileState == eTILESTATE.Empty)
                {
                    awayNode = startNode._TopRight;
                }
                else if(startNode._BottomRight != null && startNode._BottomRight._TileState == eTILESTATE.Empty)
                {
                    awayNode = startNode._BottomRight;
                }
            }
            else
            {
                if (startNode._TopLeft != null && startNode._TopLeft._TileState == eTILESTATE.Empty)
                {
                    awayNode = startNode._TopLeft;
                }
                else if (startNode._BottomLeft != null && startNode._BottomLeft._TileState == eTILESTATE.Empty)
                {
                    awayNode = startNode._BottomLeft;
                }
            }
        }

        return awayNode;
        
    }
    public void SetNodeState(int x, int y, eTILESTATE state)
    {
        GetNode(x, y).SetTileState(state);
    }
}
