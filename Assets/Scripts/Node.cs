using UnityEngine;

[System.Serializable]

public class Node
{
    public Vector3Int coordinates;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node connectedTo;

    public Node(Vector3Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
