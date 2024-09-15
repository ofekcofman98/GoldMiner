using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    [SerializeField] private float line_width = 0.05f;
    private LineRenderer lineRenderer;
    private Transform clawTransform;
    private float initialY;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = line_width;
        lineRenderer.enabled = false;
        initialY = transform.position.y;
    }

    public void SetClawTransform(Transform claw)
    {
        clawTransform = claw;
    }


    public void RenderLine(Vector3 startPosition, Vector3 endPosition, bool enableRenderer)
    {
        if (enableRenderer)
        {
            if(!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }

            lineRenderer.positionCount = 2; // start and end
        }

        else
        {
            lineRenderer.positionCount = 0;
            if(lineRenderer.enabled)
            {
                lineRenderer.enabled = false;                
            }
            return;
        }

        if (lineRenderer.enabled)
        {
            Vector3 startPos = startPosition;
            Vector3 endPos = endPosition;
                        
            startPos.z = 0f;
            endPos.z = 0f;


            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}
