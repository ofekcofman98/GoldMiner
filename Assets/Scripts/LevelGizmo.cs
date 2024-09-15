using System.Collections;
using System.Collections.Generic;
using UnityEditor; 
using UnityEngine;

[ExecuteInEditMode] // Allows the script to run in the Editor
public class LevelGizmo : MonoBehaviour
{
    public Level levelData; // Assign your Level ScriptableObject here
    public Color gizmoColor = Color.green; // Color for the gizmos
    public float gizmoSize = 0.5f; // Size of the gizmo spheres
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        if (levelData == null) return;

        Gizmos.color = gizmoColor;
    

        foreach (var itemPosition in levelData.itemPositions)
        {
            Vector3 position = new Vector3(itemPosition.position.x, itemPosition.position.y, 0);
            Gizmos.DrawSphere(position, gizmoSize);

            // Draw the item name above the gizmo for better visualization
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            Handles.Label(position + Vector3.up * 0.5f, itemPosition.itemData.itemName, style);
        }

    }
}
