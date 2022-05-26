using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGridManager : MonoBehaviour
{
    public static PathGridManager Instance;
    
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private float nodeHalfWidth;
    
    private Node[,] _grid;
    private float _nodeWidth;
    private int _numNodesX;
    private int _numNodesY;

    public int Size
    {
        get
        {
            return _numNodesX * _numNodesY;
        }
    }

    private void CreateGrid()
    {
        _grid = new Node[_numNodesX, _numNodesY];
        Vector3 gridBottomLeft = transform.position + (Vector3.left * gridSize.x / 2) + (Vector3.down * gridSize.y / 2);

        for (int x = 0; x < _numNodesX; ++x)
        {
            for (int y = 0; y < _numNodesY; ++y)
            {
                Vector3 nodePos = gridBottomLeft + Vector3.right * (x * _nodeWidth + nodeHalfWidth) +
                                  Vector3.up * (y * _nodeWidth + nodeHalfWidth);
                bool isBlocked = Physics2D.OverlapBox(nodePos, new Vector2(_nodeWidth, _nodeWidth), 0, obstacleLayerMask);
                
                _grid[x, y] = new Node(isBlocked, nodePos, x, y);
            }
        }
    }

    public Node NodeFromWorldPos(Vector3 worldPos)
    {
        float fX = (worldPos.x + gridSize.x / 2) / gridSize.x;
        float fY = (worldPos.y + gridSize.y / 2) / gridSize.y;

        fX = Mathf.Clamp01(fX);
        fY = Mathf.Clamp01(fY);

        int x = Mathf.RoundToInt((_numNodesX - 1) * fX);
        int y = Mathf.RoundToInt((_numNodesY - 1) * fY);

        return _grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < _numNodesX && checkY >= 0 && checkY < _numNodesY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.GridX - b.GridX);
        int distY = Mathf.Abs(a.GridY - b.GridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
    
    public List<Node> RetracePath(Node start, Node end)
    {
        List<Node> aPath = new List<Node>();
        Node currentNode = end;

        while (currentNode != start)
        {
            aPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        
        aPath.Reverse();
        return aPath;
    }
    
    public List<Node> FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = NodeFromWorldPos(startPos);
        Node endNode = NodeFromWorldPos(endPos);
        List<Node> currentPath = new List<Node>();

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost)
                {
                    if (openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return currentPath = RetracePath(startNode, endNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (neighbour.IsBlocked || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCost = currentNode.GridX + GetDistance(currentNode, neighbour);

                if (newMovementCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCost;
                    neighbour.HCost = GetDistance(neighbour, endNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return currentPath;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _nodeWidth = nodeHalfWidth * 2;
        _numNodesX = Mathf.RoundToInt(gridSize.x / _nodeWidth);
        _numNodesY = Mathf.RoundToInt(gridSize.y / _nodeWidth);
        CreateGrid();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));
        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = node.IsBlocked ? Color.red : Color.white;
                Gizmos.DrawWireCube(node.Position, Vector3.one * (_nodeWidth - 0.1f));
            }
        }
    }
}
