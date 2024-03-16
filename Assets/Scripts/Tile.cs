using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private float highlightAlpha;
    [SerializeField] private GameObject characterPrefab;

    private Renderer gridRenderer;
    private float originalAlpha;
    private bool isCharacterInsideTile;
    private string characterTag = "Character";

    private Vector3 mousePosition;
    private Vector3 characterPosition;
    void Start()
    {
        gridRenderer = GetComponent<Renderer>();
        originalAlpha = gridRenderer.material.color.a;
    }

    void Update()
    {
        IsMouseInsideTile();
        IsCharacterInsideTile();
    }

    // Check if something is inside the Tile
    private bool IsSomethingIntheTile(Vector3 position)
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.Contains(position);
        }

        return false;
    }

    //Check if mouse is inside a tile 
    private void IsMouseInsideTile()
    {
        mousePosition = MouseWorldPosition.Instance.GetMouseWorldPosition();

        if (IsSomethingIntheTile(mousePosition))
        {
            // Mouse is over the grid sets the grid to highlightAlpha value
            Color highlightColor = gridRenderer.material.color;
            highlightColor.a = highlightAlpha;
            gridRenderer.material.color = highlightColor;
            //Debug.Log($"Mouse entered tile: {transform.name}");
            SpawnPlayerCharacterInsideTile();
        }
        else
        {
            // Mouse is not over the grid, revert to original alpha
            Color originalColor = gridRenderer.material.color;
            originalColor.a = originalAlpha;
            gridRenderer.material.color = originalColor;
        }
    }

    //Check if Character is inside a tile
    private void IsCharacterInsideTile()
    {
        characterPosition = Character.Instance.GetCharacterPosition();

        if (IsSomethingIntheTile(characterPosition))
        {
            //Debug.Log($"Character Inside tile {transform.name}");
            isCharacterInsideTile = true;
        }
        else
        {
            isCharacterInsideTile = false;
        }
    }

    public void SpawnPlayerCharacterInsideTile()
    {

        if (PlayerInput.Instance.LeftClickClicked && IsSomethingIntheTile(mousePosition))
        {
            if (!isCharacterInsideTile)
            {
                // Instantiate the character prefab at the tile's position
                Instantiate(characterPrefab, transform.position, Quaternion.identity);
                Debug.Log(Character.Instance.CountCharactersInScene(characterTag));
            }
        }
    }
}
