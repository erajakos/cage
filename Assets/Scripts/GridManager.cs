using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public TileManager tileManager;

    public Tilemap tilemapGround;
    public Tilemap tilemapTrees;

    [SerializeField]
    private int rows = 50;

    [SerializeField]
    private int cols = 50;

    private int numGenerated = 0;
    private const float tileHeight = 0.5795f;

    private LandmassGrid landmassGrid;

    void Start()
    {
        landmassGrid = new LandmassGrid(cols, rows);
        GenerateGrid();
        //InvokeRepeating("GenerateGrid", 0.15f, 0.15f);
    }

    void GenerateGrid()
    {
        numGenerated++;
        float[,] gridData = landmassGrid.GenerateGrid(numGenerated);

        tilemapGround.ClearAllTiles();
        tilemapTrees.ClearAllTiles();


        for (int row = 0; row < gridData.GetLength(0); row++)
        {
            for (int col = 0; col < gridData.GetLength(1); col++)
            {
                float sample = gridData[col, row];
                if (sample < 0.6)
                {
                    Tile tile = GetTile(sample);
                    tilemapGround.SetTile(new Vector3Int(col, row, 0), tile);

                    if (sample >= 0.3f && sample < 0.5f && Random.Range(0, 100) >= 90)
                    {
                        tilemapTrees.SetTile(new Vector3Int(col, row, 0), tileManager.GetTile("tree"));
                    }
                }
            }
        }

        tilemapGround.transform.position = new Vector2(0, -tilemapGround.size.y / 2 * tileHeight + tileHeight / 2);
        tilemapTrees.transform.position = new Vector2(0, -tilemapGround.size.y / 2 * tileHeight + tileHeight / 2);
    }

    Tile GetTile(float sample)
    {
        if (sample < 0.3f) {
            return tileManager.GetTile("snowGrassNoShadow");
            //return tileManager.GetTile("snowGround");
        } else if (sample < 0.5f)
            return tileManager.GetTile("grassGround");
        else {
            return tileManager.GetTile("ground");
        }
    }
}
