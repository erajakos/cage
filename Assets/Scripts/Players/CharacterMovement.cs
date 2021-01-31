using System;
using System.Collections.Generic;
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
    private StrategyManager strategyManager;

    void Awake()
    {
        rm = ReferenceManager.Instance;
        grid = rm.grid;
        tilemap = rm.groundTileMap;
        movementManager = rm.movementManager;
        gridManager = rm.gridManager;
        positionManager = rm.positionManager;
        strategyManager = rm.strategyManager;

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
        if (Input.GetMouseButtonDown(0) && !moving && gameObject.tag == "Human")
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int gridPos = tilemap.WorldToCell(worldPoint);

            if (tilemap.HasTile(gridPos) && movementManager.IsValidMove(gridPos))
            {
                MoveToCell(gridPos);
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
            Vector3Int tile;
            if (gameObject.tag == "Human" || gameObject.tag == "Lemming")
            {
                Vector3Int gridCenter = new Vector3Int((int)gridManager.Rows / 2, (int)gridManager.Cols / 2, 0);
                tile = gridManager.GetClosestFreeTile(gridCenter);
            } else
            {
                tile = gridManager.GetRandomFreeBorderTile();
            }

            Vector3 cellCenter = grid.GetCellCenterWorld(tile);

            Vector2 initialPosition = new Vector2(
                cellCenter.x,
                cellCenter.y + yOffset
            );
            transform.position = initialPosition;

            em.Dispatch(new CharacterMovedEvent
            {
                character = gameObject,
                gridPos = tile
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

            if (gameObject.tag == "Enemy")
            {
                Invoke("MoveEnemy", 1);
            } else if (gameObject.tag == "Lemming")
            {
                Invoke("MoveLemming", 1);
            }
        }
        else
        {
            enabled = false;
        }
    }

    private void MoveToCell(Vector3Int cell)
    {
        Vector3 cellCenter = grid.GetCellCenterWorld(cell);
        targetPosition = new Vector2(
            cellCenter.x,
            cellCenter.y + yOffset
        );
        moving = true;

        em.Dispatch(new CharacterMovedEvent
        {
            character = gameObject,
            gridPos = cell,
        });
    }

    private void MoveEnemy()
    {
        List<Vector3Int> movementOptions = movementManager.GetMovementOptions();
        if (movementOptions.Count == 0)
        {
            em.Dispatch(new CharacterTurnEndEvent
            {
                character = gameObject
            });
        }
        else
        {
            GameObject nearestLemming = positionManager.FindNearestCharacter(transform.position, "Lemming");
            MoveToCell(strategyManager.CalculateNextMove(movementOptions, nearestLemming.transform.position));
        }
    }

    private void MoveLemming()
    {
        System.Random random = new System.Random();
        List<Vector3Int> movementOptions = movementManager.GetMovementOptions();
        if (movementOptions.Count == 0)
        {
            em.Dispatch(new CharacterTurnEndEvent
            {
                character = gameObject
            });
        }
        else
        {
            int index = random.Next(movementOptions.Count);
            MoveToCell(movementOptions[index]);
        }
    }
}
