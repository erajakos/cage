using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Landmass;

public class GridManager : MonoBehaviour
{
    public TileManager tileManager;
    public PositionManager positionManager;

    public Tilemap tilemapGround;
    public Tilemap tilemapTrees;

    [SerializeField]
    private int rows = 20;

    [SerializeField]
    private int cols = 20;

    private const float tileHeight = 0.5795f;

    private LandmassGrid landmassGrid;

    public int Rows
    {
        get { return rows; }
    }

    public int Cols
    {
        get { return cols; }
    }

    private void Start()
    {
        landmassGrid = new LandmassGrid(cols, rows);
        GenerateGrid();
        //InvokeRepeating("GenerateGrid", 0.15f, 0.15f);
    }

    private void GenerateGrid()
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

                    if (land.sample >= 0.3f && land.sample < 0.5f && UnityEngine.Random.Range(0, 100) >= 95)
                    {
                        tilemapTrees.SetTile(new Vector3Int(col, row, 0), tileManager.GetTile("tree"));
                    }
                }
            }
        }
    }

    private Tile GetTile(string type)
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

    public Vector3Int GetClosestFreeTile(Vector3Int pos)
    {
        if (tilemapGround.HasTile(pos) && !positionManager.hasCharacter(pos))
        {
            return pos;
        }

        int distance = 0;
        while (distance < Mathf.Max(Rows, Cols))
        {
            for (int x = -distance - 1; x <= distance + 1; x++)
            {
                for (int y = -distance - 1; y <= distance + 1; y++)
                {
                    Vector3Int gridPos = new Vector3Int(pos.x + x, pos.y + y, 0);
                    if (tilemapGround.HasTile(gridPos) && !positionManager.hasCharacter(gridPos))
                    {
                        return gridPos;
                    }
                }
            }
            distance++;
        }

        Debug.Log("Couldn't find suitable tile");
        return pos;
    }

    public Vector3Int GetRandomFreeBorderTile()
    {
        int x;
        int y;

        System.Random rnd = new System.Random();
        
        if (rnd.Next(0, 2) == 0)
        {
            x = rnd.Next(0, Rows);
            if (rnd.Next(0, 2) == 0)
            {
                y = 0;
            } else
            {
                y = Cols - 1;
            }
        } else
        {
            y = rnd.Next(0, Cols);
            if (rnd.Next(0, 2) == 0)
            {
                x = 0;
            }
            else
            {
                x = Rows - 1;
            }
        }

        Vector3Int pos = new Vector3Int(x, y, 0);
        return GetClosestFreeTile(pos);
    }
}
