using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Size")]
    [Space(10)]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int cellSize;
    [Space(10)]
    [SerializeField] private Tile tilePrefab;

    private float _tileOffset = 0.01f;
    public float TileOffset { get { return _tileOffset; } private set { _tileOffset = value; } }

    private Tile placedTile;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                placedTile = Instantiate(tilePrefab, new Vector3(x * cellSize, TileOffset, z * cellSize), Quaternion.identity);
                placedTile.name = $"Tile ({x},{z})";
            }
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetCellSize()
    {
        return cellSize;
    } 
}
