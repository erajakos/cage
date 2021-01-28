using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectTile : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Grid grid;

    private Camera cam;
    private float tileOffsetY = -0.12f;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // get the collision point of the ray with the z = 0 plane
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int gridPos = groundTilemap.WorldToCell(worldPoint);

        if (groundTilemap.HasTile(gridPos))
        {
            Vector3Int cellCoords = grid.WorldToCell(worldPoint);
            Vector3 cellCenter = grid.GetCellCenterWorld(cellCoords);
            transform.position = new Vector2(cellCenter.x, cellCenter.y + tileOffsetY);
        }
    }
}
