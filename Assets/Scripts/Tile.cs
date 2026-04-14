using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] bool isWalkable = true;
    [SerializeField] Tower towerPrefab;

    GridManager gridManager;
    Vector2Int coordinates = new Vector2Int();

    PathFinder pathFinder;

    public bool IsPlaceable
    {
        get
        {
            return isPlaceable;
        }
    }


    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        pathFinder = FindFirstObjectByType<PathFinder>();
    }

    void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoorditnesFromPosition(transform.position);

            if (!isWalkable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseDown()
    {
        if (!IsPlaceable) return;

        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            //Debug.Log($"clicked on: {transform.name}");
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
            if (isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyRecivers();
            }
        }
    }
}
