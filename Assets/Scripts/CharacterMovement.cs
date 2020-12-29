using UnityEngine;
using UnityEngine.Tilemaps;
using Events;
using Characters;

public class CharacterMovement : MonoBehaviour
{
    public Character character;
    private Tilemap tilemap;
    private Grid grid;
    private Camera cam;
    private bool moving;
    private float speed = 3.0f;
    private Vector2 targetPosition;
    private ReferenceManager rm;
    private EventManager em;

    void Awake()
    {
        rm = ReferenceManager.Instance;
        grid = rm.grid;
        tilemap = rm.groundTileMap;
        cam = Camera.main;
        em = EventManager.GetInstance();
    }

    void Start()
    {
        moving = false;
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
                Vector3Int cellCoords = grid.WorldToCell(worldPoint);
                Vector3 cellCenter = grid.GetCellCenterWorld(cellCoords);
                targetPosition = new Vector2(cellCenter.x, cellCenter.y + transform.lossyScale.y / 4f);
                moving = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, targetPosition) < 0.0001f)
            {
                moving = false;
                em.Dispatch(new CharacterTurnEndEvent
                {
                    character = character
                });
            }
        }
    }
}
