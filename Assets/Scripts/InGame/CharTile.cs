using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTile : MonoBehaviour
{
    Tile _onTile = null;
    Tile _targetTile = null;
    HexNode _node = null;
    public HexNode _Node => _node;

    public Vector2 _tilePos { get; private set; }

    [SerializeField] bool _isOnGame = false;

    public void SetTargetTileInfo(eTILESTATE state, GameObject charGo)
    {
        _targetTile.SetCharObj(charGo);
        _targetTile.SetTileState(state);
    }
    public void SetOnTileInfo(eTILESTATE state, GameObject charGo)
    {
        _onTile.SetCharObj(charGo);
        _onTile.SetTileState(state);
    }
    public void ChangeTile()
    {
        
        if (_targetTile != null)
        {

            UpdateOnTile(_targetTile);
        }
        _targetTile = null;

    }
    public void SetTargetTile(Tile target)
    {
        _targetTile = target;
    }
    /*
    public void SetTargetTileObj(GameObject obj)
    {
        _targetTile.SetCharObj(obj);
    }
    public void SetTargetTileState(eTILESTATE state)
    {
        _targetTile.SetTileState(state);
    }

    public void SetOnTileObj(GameObject obj)
    {
        _onTile.SetCharObj(obj);
    }
    */

    public void UpdateOnTile(Tile tile)
    {
        _onTile = tile;

        _node = tile._hexNode;

        if(_node != null)
        {
            _tilePos = new Vector3(_node._XPos, _node._YPos);
            _isOnGame = true;
        }
        else
        {
            _isOnGame = false;
        }
        
    }
    public void Dead()
    {
        _onTile.SetCharObj(null);
        _onTile.SetTileState(eTILESTATE.Empty);
    }
}
