using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] public int score;
    [SerializeField] public float size;
    private Transform clawTransform;

    public bool AlreadyDestroyed {get; set;}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Claw"))
        {
            Collect();
            GameManager.Instance.OnItemClawCollision(/*other.GetComponent<Claw>(),*/ this);
            
            clawTransform = other.transform;
            transform.SetParent(clawTransform);

        }

    }

    public virtual void Collect()
    {
        Debug.Log("Item collected!");
        // if gold / diamond / rock:
    }
}
