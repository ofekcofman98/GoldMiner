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
    public float spriteSize = 1.0f; // Size of the sprite in world space

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

            // Draw the sprite at the correct world position
            if (itemPosition.itemData.sprite != null)
            {
                Sprite sprite = itemPosition.itemData.sprite;
                Vector3 spritePos = position;

                // Convert world position to screen position for the texture to be drawn in the Scene view
                Vector3 screenPos = HandleUtility.WorldToGUIPoint(spritePos);

                // Calculate the UV rect of the sprite in the atlas (or within the sliced texture)
                Rect spriteRect = sprite.textureRect;
                Rect uvRect = new Rect(
                    spriteRect.x / sprite.texture.width,
                    spriteRect.y / sprite.texture.height,
                    spriteRect.width / sprite.texture.width,
                    spriteRect.height / sprite.texture.height
                );

                // Draw only the portion of the texture that corresponds to this sprite
                Rect rect = new Rect(screenPos.x - (spriteSize * 50) / 2, screenPos.y - (spriteSize * 50) / 2, spriteSize * 50, spriteSize * 50);

                Handles.BeginGUI();
                GUI.DrawTextureWithTexCoords(rect, sprite.texture, uvRect);
                Handles.EndGUI();
            }
        }
    }
}
