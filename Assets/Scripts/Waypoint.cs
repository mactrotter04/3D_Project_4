using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] Tower towerPrefab;

    public bool IsPlaceable
    {
        get
        {
            return isPlaceable;
        }
    }
    void OnMouseDown()
    {
        if (isPlaceable)
        {
            //Debug.Log($"clicked on: {transform.name}");
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
            isPlaceable = !isPlaced;
        }
    }
}
