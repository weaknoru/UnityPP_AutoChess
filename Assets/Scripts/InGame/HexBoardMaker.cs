using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HexBoardMaker : MonoBehaviour
{
    static HexBoardMaker _instance;

    public static HexBoardMaker Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<HexBoardMaker>();
            return _instance;
        }
    }
    /*
    [SerializeField] TMP_InputField _xInput;
    [SerializeField] TMP_InputField _yInput;

    [SerializeField] Button _testButton;
    */
    [Header("X축 갯수"), SerializeField] int _xCount = 7;
    [Header("Y축 갯수"), SerializeField] int _yCount = 4;

    [Header("(0,0) 위치"), SerializeField] Vector3 _startPos = new Vector3(-6.6f, 0f, -5f);
    [Header("타일 X 간격"), SerializeField] float _xGap = 2.4f;
    [Header("타일 Y 간격"), SerializeField] float _yGap = 2f;
    [Header("홀짝 X 간격"), SerializeField] float _oddXGap = 1.2f;

    [Header("타일 프리펩"), SerializeField] GameObject _tilePref;

    [SerializeField] StageSetter _stageSetter;

    [SerializeField] CSVReader _csvReader;
    public HexGrid _hexGrid { get; private set; }
    List<Tile> _tileList = new List<Tile>();
    

    void Start()
    {
        //_testButton.onClick.AddListener(() => ShowNeighbor());
        _hexGrid = new HexGrid(_xCount, _yCount * 2);

        MakeHexBoard();
        _stageSetter.DebugStageSetting();

        //FindDebug(6, 2, 2);

        //HexGrid hexGrid = new HexGrid(8, 7);
        //HexNode node = hexGrid.GetNode(2, 3);

        /*
        // 노드의 위치와 인접 노드 확인
        Debug.Log($"Node at (2, 3):");
        Debug.Log($"TopLeft: {node.TopLeft?.Row}, {node.TopLeft?.Col}");
        Debug.Log($"TopRight: {node.TopRight?.Row}, {node.TopRight?.Col}");
        Debug.Log($"Left: {node.Left?.Row}, {node.Left?.Col}");
        Debug.Log($"Right: {node.Right?.Row}, {node.Right?.Col}");
        Debug.Log($"BottomLeft: {node.BottomLeft?.Row}, {node.BottomLeft?.Col}");
        Debug.Log($"BottomRight: {node.BottomRight?.Row}, {node.BottomRight?.Col}");
        */
    }
    /*
    void FindDebug(int xPos, int yPos, int range)
    {
        Pathfinding pathfinding = new Pathfinding(_hexGrid);

        List<HexNode> path = pathfinding.FindPath(xPos, yPos, range);
        for(int i = 0; i < range; i++)
        {
            path.RemoveAt(path.Count - 1);
        }
        if (path != null)
        {
            foreach (var node in path)
            {
                Debug.Log($"Path Node: {node._XPos}, {node._YPos}");
            }
        }
        else
        {
            Debug.Log("경로를 찾을 수 없습니다.");
        }
    }
    */
    void MakeHexBoard()
    {
        Vector3 spawnPos = _startPos;
        float oddX = 0f;
        for (int i = 0; i < _yCount; i++)
        {
            oddX = (i % 2 == 0) ? 0f : -1.2f;
            for (int j = 0; j < _xCount; j++)
            {
                
                spawnPos = _startPos + new Vector3(j * _xGap + oddX, 0f, i * _yGap);
                GameObject tileObj = Instantiate(_tilePref, spawnPos, Quaternion.identity);
                ShaderManager.Instance.AddTileTrs(tileObj.transform);
                TileInfo tileInfo = tileObj.GetComponentInChildren<TileInfo>();
                tileInfo.SetIsPlayer(true);

                HexNode node = _hexGrid.GetNode(j, i);
                tileInfo.SetHexNode(node);
                node.SetTilePos(spawnPos);

                SetTileList(tileInfo);

            }
        }
        for (int i = _yCount; i < _yCount * 2; i++)
        {
            oddX = (i % 2 == 0) ? 0f : -1.2f;
            for (int j = 0; j < _xCount; j++)
            {

                spawnPos = _startPos + new Vector3(j * _xGap + oddX, 0f, i * _yGap);
                GameObject tileObj = Instantiate(_tilePref, spawnPos, Quaternion.identity);
                TileInfo tileInfo = tileObj.GetComponentInChildren<TileInfo>();
                tileInfo.SetIsPlayer(false);
                tileInfo.SetHexNode(_hexGrid.GetNode(j, i));

                HexNode node = _hexGrid.GetNode(j, i);
                tileInfo.SetHexNode(node);
                node.SetTilePos(spawnPos);

                SetTileList(tileInfo);
            }
        }


    }
    public void SetEnemyUnit(eCHARTYPE type, int level, Vector2 pos)
    {
        GameObject findedGo = CharManager.Instance.FindGoByType(type);
        GameObject go = Instantiate(findedGo);


        CharInfo charInfo = go.GetComponent<CharInfo>();
        charInfo.SetState(eTILESTATE.Enemy);

        charInfo.SetMovable(false, null);

        charInfo.UpdateLevel(level);
        charInfo.SetStat(_csvReader._CharData._CharDatas[type].GetStat(level));


        charInfo.SetHealth(false);

        

        Tile tile = FindTileByPos(pos);
        tile.PutCharacter(go);
        tile.SetCharObj(go);
        tile.SetTileState(eTILESTATE.Enemy);

        CharTile charTile = go.GetComponent<CharTile>();
        charTile.UpdateOnTile(tile);

        CharManager.Instance.AddEnemyTrses(go.transform);
    }
    void SetTileList(Tile tile)
    {
        _tileList.Add(tile);
    }
    Tile FindTileByPos(Vector2 pos)
    {
        Tile tile = null;
        foreach (Tile t in _tileList)
        {
            if (t._hexNode._XPos == pos.x)
            {
                if (t._hexNode._YPos == pos.y)
                {
                    tile = t;
                }
            }
        }
        return tile;
    }
    public Tile GetTileByNode(HexNode node)
    {
        Tile tile = null;
        foreach(Tile t in _tileList)
        {
            if(t._hexNode == node)
            {
                tile = t;
            }
        }
        return tile;
    }
    public int GetDistance(HexNode from, HexNode to)
    {
        int dist = 0;


        return dist;
    }
    /*
    void ShowNeighbor()
    {
        string rowText = _xInput.text;
        string colText = _yInput.text;
        int row = int.Parse(rowText);
        int col = int.Parse(colText);
        HexNode node = _hexGrid.GetNode(row, col);

        Debug.Log($"TopLeft: {node._TopLeft?._XPos}, {node._TopLeft?._YPos}");
        Debug.Log($"TopRight: {node._TopRight?._XPos}, {node._TopRight?._YPos}");
        Debug.Log($"Left: {node._Left?._XPos}, {node._Left?._YPos}");
        Debug.Log($"Right: {node._Right?._XPos}, {node._Right?._YPos}");
        Debug.Log($"BottomLeft: {node._BottomLeft?._XPos}, {node._BottomLeft?._YPos}");
        Debug.Log($"BottomRight: {node._BottomRight?._XPos}, {node._BottomRight?._YPos}");
    }
    */
}
