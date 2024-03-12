using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition Instance;

    [SerializeField] private LayerMask planeLayerMask;
    [SerializeField] private GameObject mousePositionVisual;

    private void Awake()
    {
        Instance = this;
    }

    //gets the position of the mouse from the main camera ignoring everything exept the layerMask
    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, planeLayerMask);

        //for Testing
        mousePositionVisual.transform.position = raycastHit.point;
        Debug.Log(raycastHit.point);

        return raycastHit.point;
    }
}
