using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private float line_width = 0.05f;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = line_width;
        lineRenderer.enabled = false;
    }

    public void RenderLine(Vector3 endPosition, bool enableRenderer)
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
            Vector3 startPos = startPosition.position;
            startPos.z = 0f;

            Vector3 endPos = endPosition;
            endPos.z = 0f;

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}
