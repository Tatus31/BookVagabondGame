using UnityEngine;
using UnityEngine.Rendering;

public class Tile : MonoBehaviour
{
    [SerializeField] private float highlightAlpha;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private int tileValue;

    private Renderer gridRenderer;
    private GridSystem gridSystem;
    private float originalAlpha;
    private bool isCharacterInsideTile;
    private string characterTag = "Character";
    private int maxCharacterNumber = 4;

    private Vector3 mousePosition;
    private Vector3 characterPosition;


    void Start()
    {
        gridRenderer = GetComponent<Renderer>();
        originalAlpha = gridRenderer.material.color.a;

        //Make a script to hold them all!!!
        gridSystem = GameObject.FindGameObjectWithTag("Grid").gameObject.GetComponent<GridSystem>();
    }

    void Update()
    {
        IsMouseInsideTile();
        IsCharacterInsideTile();
    }

    // Check if something is inside the Tile
    private bool IsSomethingInsideTile(Vector3 position)
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

        if (IsSomethingInsideTile(mousePosition))
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

        if (IsSomethingInsideTile(characterPosition))
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
        if (PlayerInput.Instance.LeftClickClicked && IsSomethingInsideTile(mousePosition) && CheckIfPlayerControlledTile())
        {
            if (!isCharacterInsideTile)
            {
                if (Character.Instance.CountCharactersInScene(characterTag) >= maxCharacterNumber)
                {
                    return;
                }

                // Instantiate the character prefab at the tile's position
                Instantiate(characterPrefab, transform.position, Quaternion.identity);
                Debug.Log(Character.Instance.CountCharactersInScene(characterTag));

                Character.Instance.CountCharactersInScene(characterTag);
            }
        }
    }

    //Checks if placing player characters is allowed inside area
    private bool CheckIfPlayerControlledTile()
    {
        if (transform.position.x >= gridSystem.GetWidth())
            return true;
        else if (transform.position.x < gridSystem.GetWidth())
            return false;

        else return false;
    }

    public void SetTileValue(int value)
    {
        tileValue = value;
    }

    public int GetTileValue()
    {
        return tileValue;
    }
}

