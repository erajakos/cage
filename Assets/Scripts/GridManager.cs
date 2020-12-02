using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public TileManager tileManager;

    private Tilemap tilemap;
    
    [SerializeField]
    private int rows = 50;

    [SerializeField]
    private int cols = 50;

    private int numGenerated = 0;
    private const float tileHeight = 0.5795f;

    private Grid grid;

    private void Awake()
    {
        tilemap = FindObjectOfType<Tilemap>();
        grid = new Grid(cols, rows);
    }

    void Start()
    {
        GenerateGrid();
        InvokeRepeating("GenerateGrid", 0.15f, 0.15f);
    }

    void GenerateGrid()
    {
        numGenerated++;
        float[,] gridData = grid.GenerateGrid(numGenerated);

        tilemap.ClearAllTiles();

        for (int row = 0; row < gridData.GetLength(0); row++)
        {
            for (int col = 0; col < gridData.GetLength(1); col++)
            {
                float sample = gridData[col, row];
                if (sample < 0.6)
                {
                    Tile tile = GetTile(sample);
                    tilemap.SetTile(new Vector3Int(col, row, 0), tile);
                }
            }
        }

        tilemap.transform.position = new Vector2(0, -tilemap.size.y / 2 * tileHeight + tileHeight / 2);
    }

    Tile GetTile(float sample)
    {
        if (sample < 0.3f) {
            return tileManager.GetTile("snowIce");
        } else if (sample < 0.5f)
            return tileManager.GetTile("ground");
        else {
            return tileManager.GetTile("grassGround");
        }
    }
}
