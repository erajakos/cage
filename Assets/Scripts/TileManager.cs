using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private Tile grassGroundTile;
    private Tile groundTile;
    private Tile snowIceTile;
    private Tile snowGrassTile;
    private Tile snowGrassNoShadowTile;
    private Tile snowGroundTile;
    private Tile iceGroundTile;
    private Tile treeTile;

    void Awake()
    {
        grassGroundTile = (Tile)Instantiate(Resources.Load("Tiles/GrassGround"));
        groundTile = (Tile)Instantiate(Resources.Load("Tiles/Ground"));
        snowIceTile = (Tile)Instantiate(Resources.Load("Tiles/SnowIce"));
        snowGroundTile = (Tile)Instantiate(Resources.Load("Tiles/SnowGround"));
        snowGrassTile = (Tile)Instantiate(Resources.Load("Tiles/SnowGrass"));
        snowGrassNoShadowTile = (Tile)Instantiate(Resources.Load("Tiles/SnowGrassNoShadow"));
        iceGroundTile = (Tile)Instantiate(Resources.Load("Tiles/IceGround"));
        treeTile = (Tile)Instantiate(Resources.Load("Tiles/Tree"));
    }

    public Tile GetTile(string tileName)
    {
        switch (tileName) {
            case "grassGround":
                return grassGroundTile;
            case "snowIce":
                return snowIceTile;
            case "snowGround":
                return snowGroundTile;
            case "snowGrass":
                return snowGrassTile;
            case "snowGrassNoShadow":
                return snowGrassNoShadowTile;
            case "iceGround":
                return iceGroundTile;
            case "ground":
                return groundTile;
            case "tree":
                return treeTile;
            default:
                throw new System.Exception("Invalid tileName:" + tileName);
        }
    }
}
