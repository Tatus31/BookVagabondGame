using UnityEngine;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition Instance;

    [SerializeField] private LayerMask InteractableLayerMask;
    [SerializeField] private GameObject mousePositionVisual;

    private Ray ray;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetMouseWorldPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, InteractableLayerMask);

        mousePositionVisual.transform.position = raycastHit.point;
        // Debug.Log(raycastHit.point);

        return raycastHit.point;
    }

    public Ray GetMouseRayWorld()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray;
    }
}
