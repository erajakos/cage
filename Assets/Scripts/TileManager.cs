using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private Tile grassGroundTile;
    private Tile groundTile;
    private Tile snowIceTile;

    void Awake()
    {
        grassGroundTile = (Tile)Instantiate(Resources.Load("Tiles/GrassGround"));
        groundTile = (Tile)Instantiate(Resources.Load("Tiles/Ground"));
        snowIceTile = (Tile)Instantiate(Resources.Load("Tiles/SnowIce"));
    }

    public Tile GetTile(string tileName)
    {
        switch (tileName) {
            case "grassGround":
                return grassGroundTile;
            case "snowIce":
                return snowIceTile;
            case "ground":
                return groundTile;
            default:
                throw new System.Exception("Invalid tileName:" + tileName);
        }
    }
}
