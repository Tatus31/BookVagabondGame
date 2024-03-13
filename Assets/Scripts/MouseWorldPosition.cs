using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition Instance;

    [SerializeField] private LayerMask InteractableLayerMask;
    [SerializeField] private GameObject mousePositionVisual;

    private void Awake()
    {
        Instance = this;
    }

    // Gets the position of the mouse from the main camera, ignoring everything except the layerMask
    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, InteractableLayerMask);

        // For Testing
        mousePositionVisual.transform.position = raycastHit.point;
        // Debug.Log(raycastHit.point);

        return raycastHit.point;
    }
}
