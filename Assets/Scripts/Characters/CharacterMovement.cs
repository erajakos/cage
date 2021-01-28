using UnityEngine;
using UnityEngine.Tilemaps;
using Events;

public class CharacterMovement : MonoBehaviour
{
    private Tilemap tilemap;
    private Grid grid;
    private Camera cam;
    private bool moving;
    private float speed = 3.0f;
    private float yOffset = -0.2f;
    private Vector2 targetPosition;
    private ReferenceManager rm;
    private EventManager em;
    private MovementManager movementManager;
    private GridManager gridManager;
    private PositionManager positionManager;

    void Awake()
    {
        rm = ReferenceManager.Instance;
        grid = rm.grid;
        tilemap = rm.groundTileMap;
        movementManager = rm.movementManager;
        positionManager = rm.positionManager;
        gridManager = rm.gridManager;

        cam = Camera.main;
        em = EventManager.GetInstance();
        em.AddListener<CharacterAddedEvent>(OnCharacterAdded);
        em.AddListener<CharacterTurnStartEvent>(OnCharacterTurnStart);
    }

    void Start()
    {
        moving = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !moving)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int gridPos = tilemap.WorldToCell(worldPoint);

            if (tilemap.HasTile(gridPos) && movementManager.IsValidMove(gridPos))
            {
                Vector3Int cellCoords = grid.WorldToCell(worldPoint);
                Debug.Log(cellCoords);
                targetPosition = TargetCellToWorld(cellCoords);
                
                Vector3 cellCenter = grid.GetCellCenterWorld(cellCoords);
                targetPosition = new Vector2(
                    cellCenter.x,
                    cellCenter.y + yOffset
                );
                moving = true;

                em.Dispatch(new CharacterMovedEvent
                {
                    character = gameObject,
                    gridPos = gridPos,
                });
            }
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                step
            );

            // Check if the position of the cube and sphere are approximately
            // equal
            if (Vector3.Distance(transform.position, targetPosition) < 0.0001f)
            {
                moving = false;
                em.Dispatch(new CharacterTurnEndEvent
                {
                    character = gameObject
                });
            }
        }
    }

    private void OnCharacterAdded(CharacterAddedEvent e)
    {
        if (ReferenceEquals(e.character, gameObject))
        {
            Vector3Int gridCenter = new Vector3Int(0, 0, 0);
            Vector3Int closestTile = positionManager.GetClosestFreeTile(gridCenter);

            Vector3 cellCenter = grid.GetCellCenterWorld(closestTile);
            Vector2 initialPosition = new Vector2(
                cellCenter.x,
                cellCenter.y + yOffset
            );
            transform.position = initialPosition;

            em.Dispatch(new CharacterMovedEvent
            {
                character = gameObject,
                gridPos = closestTile
            });

            enabled = false;
        }
    }

    private void OnCharacterTurnStart(CharacterTurnStartEvent e)
    {
        if (ReferenceEquals(e.character, gameObject))
        {
            enabled = true;
            Vector3Int gridPos = tilemap.WorldToCell(transform.position);

            em.Dispatch(new CharacterStartPosEvent
            {
                character = gameObject,
                gridPos = gridPos,
                position = transform.position
            });
        }
        else
        {
            enabled = false;
        }
    }

    private Vector2 TargetCellToWorld(Vector3Int cellCoords)
    {
        Vector3 cellCenter = grid.GetCellCenterWorld(cellCoords);
        return new Vector2(
            cellCenter.x,
            cellCenter.y + transform.lossyScale.y / 4f
        );
    }
}
