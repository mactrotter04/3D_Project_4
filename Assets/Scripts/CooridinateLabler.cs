using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CooridinateLabler : MonoBehaviour
{
    [SerializeField] Color defultColour = Color.white;
    [SerializeField] Color blockedColour = Color.gray;
    [SerializeField] Color exploredColour = Color.steelBlue;
    [SerializeField] Color pathColour = Color.orangeRed;

    TextMeshPro label;
    Vector3Int Coordinates = new Vector3Int();
    Tile waypoint;
    GridManager gridManager;

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Tile>();
        DisplayCoodinates();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying)
        {
            DisplayCoodinates();
            UpdateObjectName();
        }

        ToggleLabels();
        SetLableColours();
    }

    void DisplayCoodinates()
    {
        Coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        Coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        Coordinates.z = Mathf.RoundToInt(transform.parent.position.y / UnityEditor.EditorSnapSettings.move.y);

        label.text = Coordinates.x + "; " + Coordinates.y + ": " + Coordinates.z;
    }

    void UpdateObjectName()
    {
        transform.parent.name = Coordinates.ToString();
    }

    void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            label.enabled = !label.IsActive();
        }
    }

    void SetLableColours()
    {
        if(gridManager == null) return;

        Node node = gridManager.GetNode(Coordinates);

        if (node == null) return;

        if(!node.isWalkable)
        {
            label.color = blockedColour;
        }

        else if(node.isPath)
        {
            label.color = pathColour;
        }

        else if(node.isExplored)
        {
            label.color = exploredColour;
        }

        else
        {
            label.color = defultColour;
        }
    }
}
