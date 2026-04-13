using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CooridinateLabler : MonoBehaviour
{
    [SerializeField] Color defultColour = Color.white;
    [SerializeField] Color blockedColour = Color.gray;

    TextMeshPro label;
    Vector2Int Coordinates = new Vector2Int();
    Waypoint waypoint;

    void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Waypoint>();
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
        ColorCoordinates();
    }

    void DisplayCoodinates()
    {
        Coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        Coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = Coordinates.x + "; " + Coordinates.y;
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

    void ColorCoordinates()
    {
        if(waypoint.IsPlaceable)
        {
            label.color = defultColour;
        }
        else
        {
            label.color = blockedColour;
        }
    }
}
