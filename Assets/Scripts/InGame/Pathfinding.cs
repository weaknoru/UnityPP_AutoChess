using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    private HexGrid _hexGrid;

    public Pathfinding(HexGrid hexGrid)
    {
        _hexGrid = hexGrid;
    }

    public List<HexNode> FindPath(int startX, int startY, int range, eTILESTATE charState, eTILESTATE targetState)
    {
        HexNode startNode = _hexGrid.GetNode(startX, startY);
        if (startNode == null) return null;

        // 1. 열린 목록과 닫힌 목록 초기화
        List<HexNode> openList = new List<HexNode>();
        HashSet<HexNode> closedList = new HashSet<HexNode>();

        // 2. 시작 노드 초기화
        Dictionary<HexNode, HexNode> parentMap = new Dictionary<HexNode, HexNode>();
        Dictionary<HexNode, float> gScore = new Dictionary<HexNode, float>();
        Dictionary<HexNode, float> fScore = new Dictionary<HexNode, float>();

        openList.Add(startNode);
        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, FindNearestEnemy(startNode, targetState));

        // 3. A* 루프
        while (openList.Count > 0)
        {
            HexNode current = openList.OrderBy(n => fScore.ContainsKey(n) ? fScore[n] : float.MaxValue).First();

            // 조건: 적 노드에서 range 거리만큼 떨어진 위치에 도달하면 경로 반환
            if (current._TileState == targetState)
            {
                return ReconstructPath(parentMap, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (HexNode neighbor in GetNeighbors(current))
            {
                /*
                if (closedList.Contains(neighbor) || neighbor._TileState == charState || neighbor._TileState == eTILESTATE.Wait)
                {
                    continue;
                }
                */
                if (closedList.Contains(neighbor) || neighbor._TileState == charState)
                {
                    continue;
                }
                float tentativeGScore = gScore[current] + 1;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                parentMap[neighbor] = current;
                gScore[neighbor] = tentativeGScore;

                // fScore는 현재까지의 비용 + 휴리스틱 추정치
                fScore[neighbor] = tentativeGScore + Heuristic(neighbor, FindNearestEnemy(startNode, targetState));
            }
        }

        // 경로를 찾지 못한 경우
        return null;
    }
    public List<HexNode> FindPath(int startX, int startY, int range, eTILESTATE charState, GameObject targetGo)
    {
        HexNode startNode = _hexGrid.GetNode(startX, startY);
        if (startNode == null) return null;

        // 1. 열린 목록과 닫힌 목록 초기화
        List<HexNode> openList = new List<HexNode>();
        HashSet<HexNode> closedList = new HashSet<HexNode>();

        // 2. 시작 노드 초기화
        Dictionary<HexNode, HexNode> parentMap = new Dictionary<HexNode, HexNode>();
        Dictionary<HexNode, float> gScore = new Dictionary<HexNode, float>();
        Dictionary<HexNode, float> fScore = new Dictionary<HexNode, float>();

        openList.Add(startNode);
        gScore[startNode] = 0;
        fScore[startNode] = Heuristic(startNode, _hexGrid.FindNodeByGameObject(targetGo));

        // 3. A* 루프
        while (openList.Count > 0)
        {
            HexNode current = openList.OrderBy(n => fScore.ContainsKey(n) ? fScore[n] : float.MaxValue).First();

            // 조건: 적 노드에서 range 거리만큼 떨어진 위치에 도달하면 경로 반환
            if (current._charGo == targetGo)
            {
                return ReconstructPath(parentMap, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (HexNode neighbor in GetNeighbors(current))
            {
                /*
                if (closedList.Contains(neighbor) || neighbor._TileState == charState || neighbor._TileState == eTILESTATE.Wait)
                {
                    continue;
                }
                */
                eTILESTATE targetState = (charState == eTILESTATE.Player) ? eTILESTATE.Enemy : eTILESTATE.Player;
                if (closedList.Contains(neighbor) || neighbor._TileState == charState || (neighbor._TileState == targetState && neighbor._charGo != targetGo))
                {
                    continue;
                }
                float tentativeGScore = gScore[current] + 1;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                parentMap[neighbor] = current;
                gScore[neighbor] = tentativeGScore;

                // fScore는 현재까지의 비용 + 휴리스틱 추정치
                fScore[neighbor] = tentativeGScore + Heuristic(neighbor, _hexGrid.FindNodeByGameObject(targetGo));
            }
        }

        // 경로를 찾지 못한 경우
        return null;
    }
    
    private float Heuristic(HexNode a, HexNode b)
    {
        if (a == null || b == null) return float.MaxValue;
        return Mathf.Abs(a._XPos - b._XPos) + Mathf.Abs(a._YPos - b._YPos); // 맨해튼 거리
    }

    private List<HexNode> ReconstructPath(Dictionary<HexNode, HexNode> parentMap, HexNode current)
    {
        List<HexNode> path = new List<HexNode>();
        while (parentMap.ContainsKey(current))
        {
            path.Add(current);
            current = parentMap[current];
        }
        path.Reverse();
        return path;
    }

    private List<HexNode> GetNeighbors(HexNode node)
    {
        return new List<HexNode>
        {
            node._TopLeft, node._TopRight, node._Left, node._Right, node._BottomLeft, node._BottomRight
        }.Where(n => n != null).ToList();
    }

    public HexNode FindNearestEnemy(HexNode startNode, eTILESTATE targetState)
    {
        Dictionary<HexNode, float> nodeList = new Dictionary<HexNode, float>();
        for (int x = 0; x < _hexGrid._xSize; x++)
        {
            for (int y = 0; y < _hexGrid._ySize; y++)
            {
                HexNode node = _hexGrid.GetNode(x, y);
                if (node._TileState == targetState)
                {
                    nodeList.Add(node, Heuristic(node, startNode));
                    //return node; // 첫 번째로 발견한 적
                }
            }
        }
        if (nodeList.Count > 0)
        {
            float nearestDist = nodeList.First().Value;
            HexNode nearestNode = nodeList.OrderBy(pair => pair.Value).First().Key;
            return nearestNode;
        }
        else
        {
            return null;
        }

        
    }
    public GameObject FindNearestEnemyGo(HexNode startNode, eTILESTATE targetState)
    {
        Dictionary<HexNode, float> nodeList = new Dictionary<HexNode, float>();
        for (int x = 0; x < _hexGrid._xSize; x++)
        {
            for (int y = 0; y < _hexGrid._ySize; y++)
            {
                HexNode node = _hexGrid.GetNode(x, y);
                if (node._TileState == targetState)
                {
                    nodeList.Add(node, Heuristic(node, startNode));
                    //return node; // 첫 번째로 발견한 적
                }
            }
        }
        if (nodeList.Count > 0)
        {
            float nearestDist = nodeList.First().Value;
            HexNode nearestNode = nodeList.OrderBy(pair => pair.Value).First().Key;
            return nearestNode._charGo;
        }
        else
        {
            return null;
        }


    }
    public GameObject FindFarthestEnemyGo(HexNode startNode, eTILESTATE targetState)
    {
        Dictionary<HexNode, float> nodeList = new Dictionary<HexNode, float>();

        for (int x = 0; x < _hexGrid._xSize; x++)
        {
            for (int y = 0; y < _hexGrid._ySize; y++)
            {
                HexNode node = _hexGrid.GetNode(x, y);
                if (node._TileState == targetState)
                {
                    nodeList.Add(node, Heuristic(node, startNode));
                }
            }
        }

        if (nodeList.Count > 0)
        {
            float farthestDist = nodeList.First().Value;
            HexNode farthestNode = nodeList.OrderByDescending(pair => pair.Value).First().Key;
            return farthestNode._charGo;
        }
        else
        {
            return null;
        }
    }
    public HexNode FindFarthestNode(HexNode startNode, eTILESTATE targetState)
    {
        Dictionary<HexNode, float> nodeList = new Dictionary<HexNode, float>();

        for (int x = 0; x < _hexGrid._xSize; x++)
        {
            for (int y = 0; y < _hexGrid._ySize; y++)
            {
                HexNode node = _hexGrid.GetNode(x, y);
                if (node._TileState == targetState)
                {
                    nodeList.Add(node, Heuristic(node, startNode));
                }
            }
        }

        if (nodeList.Count > 0)
        {
            float farthestDist = nodeList.First().Value;
            HexNode farthestNode = nodeList.OrderByDescending(pair => pair.Value).First().Key;
            return farthestNode;
        }
        else
        {
            return null;
        }
    }

}
