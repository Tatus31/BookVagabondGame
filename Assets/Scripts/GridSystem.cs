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

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        float centerPosition = (width - 1) * cellSize * 0.5f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float distanceFromCenter = Mathf.Abs(x * cellSize - centerPosition);
                int tileValue = (int)distanceFromCenter;

                Tile placedTile = Instantiate(tilePrefab, new Vector3(x * cellSize, TileOffset, z * cellSize), Quaternion.identity);
                placedTile.name = $"Tile ({x},{z})";

                placedTile.SetTileValue(tileValue);
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
