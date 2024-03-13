using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer gridRenderer;
    private float originalAlpha;
    [SerializeField] private float highlightAlpha;

    void Start()
    {
        gridRenderer = GetComponent<Renderer>();
        originalAlpha = gridRenderer.material.color.a;
    }

    void Update()
    {
        // Get the mouse position from the MouseWorldPosition script
        Vector3 mousePosition = MouseWorldPosition.Instance.GetMouseWorldPosition();

        // Check if the mouse is over the grid
        if (IsMouseOverGrid(mousePosition))
        {
            // Mouse is over the grid, set the alpha to the highlight value
            Color highlightColor = gridRenderer.material.color;
            highlightColor.a = highlightAlpha;
            gridRenderer.material.color = highlightColor;
            Debug.Log($"Mouse entered tile: {transform.name}");
        }
        else
        {
            // Mouse is not over the grid, revert to original alpha
            Color originalColor = gridRenderer.material.color;
            originalColor.a = originalAlpha;
            gridRenderer.material.color = originalColor;
        }
    }

    // Check if the mouse is over the grid
    private bool IsMouseOverGrid(Vector3 mousePosition)
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.Contains(mousePosition);
        }

        return false;
    }
}
