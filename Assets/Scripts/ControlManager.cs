using UnityEngine;
using UnityEngine.Tilemaps;

public class ControlManager : MonoBehaviour
{
    public Tilemap tilemap;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int gridPos = tilemap.WorldToCell(worldPoint);
            if (tilemap.HasTile(gridPos))
            {
                Debug.Log("Hello World from " + gridPos);
            }
        }
    }
}
