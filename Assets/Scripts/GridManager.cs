using UnityEngine;
using UnityEngine.Tilemaps;
using Landmass;

public class GridManager : MonoBehaviour
{
    public TileManager tileManager;

    public Tilemap tilemapGround;
    public Tilemap tilemapTrees;

    [SerializeField]
    private int rows = 50;

    [SerializeField]
    private int cols = 50;

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
        Land[,] gridData = landmassGrid.GenerateGrid(UnityEngine.Random.Range(0,1000000));

        tilemapGround.ClearAllTiles();
        tilemapTrees.ClearAllTiles();

        for (int row = 0; row < gridData.GetLength(0); row++)
        {
            for (int col = 0; col < gridData.GetLength(1); col++)
            {
                Land land = gridData[col, row];
                if (land.type != "water")
                {
                    Tile tile = GetTile(land.type);
                    tilemapGround.SetTile(new Vector3Int(col, row, 0), tile);

                    /*
                    if (land.sample >= 0.3f && land.sample < 0.5f && UnityEngine.Random.Range(0, 100) >= 95)
                    {
                        tilemapTrees.SetTile(new Vector3Int(col, row, 0), tileManager.GetTile("tree"));
                    }
                    */
                }
            }
        }

        tilemapGround.transform.position = new Vector2(0, -tilemapGround.size.y / 2 * tileHeight + tileHeight / 2);
        tilemapTrees.transform.position = new Vector2(0, -tilemapGround.size.y / 2 * tileHeight + tileHeight / 2);
    }

    Tile GetTile(string type)
    {
        if (type == "snow-grass")
        {
            return tileManager.GetTile("snowGrass");
            //return tileManager.GetTile("snowGround");
        }
        else if (type == "grass-ground")
        {
            return tileManager.GetTile("grassGround");
        }
        else
        {
            return tileManager.GetTile("ground");
        }
    }
}
