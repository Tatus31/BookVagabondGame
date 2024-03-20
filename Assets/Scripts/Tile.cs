using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [SerializeField] private float highlightAlpha;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private int tileValue;

    private Renderer gridRenderer;
    private GridSystem gridSystem;
    private float originalAlpha;

    private List<GameObject> charactersOnTile = new List<GameObject>();

    void Start()
    {
        gridRenderer = GetComponent<Renderer>();
        originalAlpha = gridRenderer.material.color.a;

        gridSystem = GameObject.FindGameObjectWithTag("Grid").gameObject.GetComponent<GridSystem>();
    }

    void Update()
    {
        IsMouseInsideTile();
    }

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
        Vector3 mousePosition = MouseWorldPosition.Instance.GetMouseWorldPosition();

        if (IsSomethingInsideTile(mousePosition))
        {
            Color highlightColor = gridRenderer.material.color;
            highlightColor.a = highlightAlpha;
            gridRenderer.material.color = highlightColor;

            if (PlayerInput.Instance.LeftClickClicked)
            {
                SpawnPlayerCharacterInsideTile();
            }
        }
        else
        {
            Color originalColor = gridRenderer.material.color;
            originalColor.a = originalAlpha;
            gridRenderer.material.color = originalColor;
        }
    }

    public void SpawnPlayerCharacterInsideTile()
    {
        if (charactersOnTile.Count < 1 && CheckIfPlayerControlledTile())
        {
            if (Character.Instance.CountCharactersInScene("Character") >= 4)
            {
                Debug.Log("Maximum characters reached in the scene.");
                return; 
            }
            GameObject newCharacter = Instantiate(characterPrefab, transform.position, Quaternion.identity);
            charactersOnTile.Add(newCharacter);           
        }
        else
        {
            Debug.Log("Maximum characters reached on this tile.");
        }
    }

    private bool CheckIfPlayerControlledTile()
    {
        if (transform.position.x >= gridSystem.GetWidth())
            return true;
        else if (transform.position.x < gridSystem.GetWidth())
            return false;

        else return false;
    }

    public void RemoveCharacterFromTile(GameObject character)
    {
        charactersOnTile.Remove(character);
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
