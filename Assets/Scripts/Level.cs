using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level", order = 2)]
public class Level : ScriptableObject
{
    public int scoreGoal;
    public ItemPosition[] itemPositions;

}

    [System.Serializable] 
    public class ItemPosition
    {
        public ItemData itemData; 
        public Vector2 position;  
    }
