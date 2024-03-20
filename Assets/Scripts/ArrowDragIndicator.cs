using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDragIndicator : MonoBehaviour
{
    [SerializeField] AnimationCurve animationCurve;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private LineRenderer lineRenderer;

    private void Update()
    {
        if (PlayerInput.Instance.LeftClickClicked)
        {
            if(lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            lineRenderer.enabled = true;    
            lineRenderer.positionCount = 2;
            startPosition = MouseWorldPosition.Instance.GetMouseWorldPosition();
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.useWorldSpace = true;

            lineRenderer.widthCurve = animationCurve;
            lineRenderer.numCapVertices = 10;
        }
        if (PlayerInput.Instance.LeftClickHeld)
        {
            endPosition = MouseWorldPosition.Instance.GetMouseWorldPosition();
            lineRenderer.SetPosition(1, endPosition);
        }
        if (PlayerInput.Instance.LeftClickUp)
        {
            lineRenderer.enabled = false;
        }
    }
}
