using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

    Node currentSearchNode;
    Node startNode;
    Node destinationNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] direction = { Vector2Int.up,
                               Vector2Int.right,
                               Vector2Int.down,
                               Vector2Int.left };

    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public Vector2Int StartCoordinates {  get { return startCoordinates; } }
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
        }

        startNode = grid[startCoordinates];
        destinationNode = grid[destinationCoordinates];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinationCoordinates];

        GetNewPath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (Vector2Int direction in direction)
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighborCoordinates))
            {
                neighbours.Add(grid[neighborCoordinates]);
            }
        }
        foreach (Node neighbor in neighbours)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();

        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public List<Node> GetNewPath(Vector2Int startCoordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(startCoordinates);
        return BuildPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;
            List<Node> NewPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(NewPath.Count <=1)
            {
               GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyRecivers()
    {
        BroadcastMessage("RecalcualtePath", SendMessageOptions.DontRequireReceiver);
    }
}
