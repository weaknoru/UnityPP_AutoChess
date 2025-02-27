using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eTILESTATE
{
    Empty,
    Wait,
    Player,
    Enemy
}
public class TileInfo : Tile
{
    [Header("타일 좌표"), SerializeField] Vector2 _tilePos;
    [Header("챔피언 배치 위치"), SerializeField] Transform _charTrs;

    public bool _isPlayerTile = false;
    public bool _IsPlayerTile => _isPlayerTile;

    Renderer _renderer;
    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }
    public void SetIsPlayer(bool isPlayerTile)
    {
        _isPlayerTile = isPlayerTile;
    }
    public void SetHexNode(HexNode tilePos)
    {
        _hexNode = tilePos;
        _tilePos = new Vector2(_hexNode._XPos, _hexNode._YPos);
    }
    public override void PutCharacter(GameObject charObj)
    {
        charObj.transform.position = _charTrs.position;
        charObj.transform.rotation = _charTrs.rotation;

        MovableUnit movable = charObj.GetComponent<MovableUnit>();
        movable.ResetHpBarPos();
    }
    public override void SetCharObj(GameObject charObj)
    {

        _onCharGo = charObj;
        _hexNode.SetCharGo(charObj);
    }
    
    private void OnMouseEnter()
    {
        ShaderManager.Instance.SetTileMatarial(_renderer, eDRAGSTATE.Selected);
    }
    private void OnMouseExit()
    {
        ShaderManager.Instance.SetTileMatarial(_renderer, eDRAGSTATE.Defalut);
    }

    public override void SetTileState(eTILESTATE state)
    {
        _state = state;
        _hexNode.SetTileState(_state);
    }
}
