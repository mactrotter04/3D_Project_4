using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector3Int startCoordinates;
    [SerializeField] Vector3Int destinationCoordinates;

    Node currentSearchNode;
    Node startNode;
    Node destinationNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector3Int, Node> reached = new Dictionary<Vector3Int, Node>();

    Vector3Int[] direction = { Vector3Int.right, Vector3Int.left,
                               Vector3Int.up, Vector3Int.down,
                               Vector3Int.forward + Vector3Int.right, Vector3Int.forward + Vector3Int.left, Vector3Int.forward + Vector3Int.up, Vector3Int.forward + Vector3Int.down,
                               Vector3Int.back + Vector3Int.right, Vector3Int.back + Vector3Int.left, Vector3Int.back + Vector3Int.up, Vector3Int.back + Vector3Int.down};

    GridManager gridManager;
    Dictionary<Vector3Int, Node> grid = new Dictionary<Vector3Int, Node>();

    public Vector3Int StartCoordinates {  get { return startCoordinates; } }
    public Vector3Int DestinationCoordinates { get { return destinationCoordinates; } }

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

        foreach (Vector3Int direction in direction)
        {
            Vector3Int neighborCoordinates = currentSearchNode.coordinates + direction;

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

    void BreadthFirstSearch(Vector3Int coordinates)
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

    public List<Node> GetNewPath(Vector3Int startCoordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(startCoordinates);
        return BuildPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public bool WillBlockPath(Vector3Int coordinates)
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
