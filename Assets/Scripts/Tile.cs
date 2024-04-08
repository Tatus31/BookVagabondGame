using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;

public class Tile : MonoBehaviour
{
    public static Tile Instance { get; private set; }

    [SerializeField] private float highlightAlpha;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private int tileValue;

    private Renderer gridRenderer;
    private GridSystem gridSystem;
    private float originalAlpha;

    private bool _allCharactersPlaced;

    public bool AllCharactersPlaced { get { return _allCharactersPlaced; } private set { _allCharactersPlaced = value; } }

    private List<GameObject> charactersOnTile = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

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
            if (Character.Instance != null && Character.Instance.CountCharactersInScene("Character") >= 4)
            {
                _allCharactersPlaced = true;
                Debug.Log("Maximum characters reached");
                return;
            }
            if (characterPrefab != null)
            {
                GameObject newCharacter = Instantiate(characterPrefab, transform.position, Quaternion.identity);
                charactersOnTile.Add(newCharacter);
                CharactersAndEnemiesList.Instance.characters.Add(newCharacter);
            }
            else
            {
                Debug.LogError("Character prefab is not assigned");
            }
        }
        else
        {
            Debug.Log("Maximum characters reached on tile");
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
