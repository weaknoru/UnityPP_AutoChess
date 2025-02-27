using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : MonoBehaviour
{
    public bool _isClicked = false;
    Collider _col;

    Tile _startTile = null;

    bool _isPlayer = false;
    [Header("캐릭터 정보"), SerializeField] CharInfo _charInfo;
    [Header("캐릭터 이동"), SerializeField] CharMove _charMove;
    [Header("타일 컨트롤러"), SerializeField] CharTile _charTile;

    MoveBar _moveHpBar;

    Vector3 _startPos;
    private void Awake()
    {
        _col = GetComponent<Collider>();
        if(_charMove == null)
        {
            _charMove = GetComponent<CharMove>();
        }
        if (_charTile == null)
        {
            _charTile = GetComponent<CharTile>();
        }
    }
    public void SetMoveHpBar(MoveBar moveHpBar)
    {
        _moveHpBar = moveHpBar;
        _charMove.SetMoveHpBar(_moveHpBar);
    }
    public void ResetHpBarPos()
    {
        _moveHpBar.MoveBarObj(transform.position);
    }
    public void SetTile(Tile tile)
    {
        _startTile = tile;
    }
    public void SetIsPlayer(bool isPlayer)
    {
        _isPlayer = isPlayer;
    }
    private void OnMouseDown()
    {
        if(!_isPlayer)
        {
            return;
        }
        _isClicked = true;
        InGameDrag.Instance.SetClickedObj(gameObject);
        InGameDrag.Instance.SetClickedHpBar(_moveHpBar);
        InGameDrag.Instance.StartDrag();

        ShaderManager.Instance.SetLineTileMatarial(eDRAGSTATE.Drag);
        _col.enabled = false;
    }
    private void OnMouseUp()
    {
        if (!_isPlayer)
        {
            return;
        }
        _col.enabled = true;
        _isClicked = false;
        InGameDrag.Instance.SetClickedObj(null);
        InGameDrag.Instance.SetClickedHpBar(null);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
        {
            Transform tileTrs = hit.transform;
            TileInfo tileInfo = tileTrs.GetComponentInChildren<TileInfo>();
            if(tileInfo != null)
            {
                if (tileInfo._IsPlayerTile)
                {
                    _startTile.SetTileState(eTILESTATE.Empty);
                    _startTile.SetCharObj(null);
                    _startTile = tileInfo;

                }
                else
                {

                
                }
            }
            else
            {
                QueueInfo queueInfo = tileTrs.GetComponentInChildren<QueueInfo>();
                if(queueInfo._IsPlayerQueue)
                {
                    _startTile.SetTileState(eTILESTATE.Empty);
                    _startTile.SetCharObj(null);
                    _startTile = queueInfo;
                }
            }
            
            

        }
        _charInfo.UpdateOnTile(_startTile);
        _charTile.UpdateOnTile(_startTile);
        //SetTile(_startTile);
        _startTile.SetTileState(eTILESTATE.Player);
        ShaderManager.Instance.SetLineTileMatarial(eDRAGSTATE.Defalut);

        
    }
    
}
