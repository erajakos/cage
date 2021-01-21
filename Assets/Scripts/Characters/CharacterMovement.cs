﻿using UnityEngine;
using UnityEngine.Tilemaps;
using Events;

public class CharacterMovement : MonoBehaviour
{
    private Tilemap tilemap;
    private Grid grid;
    private Camera cam;
    private bool moving;
    private float speed = 3.0f;
    private Vector2 targetPosition;
    private ReferenceManager rm;
    private EventManager em;
    private MovementManager movementManager;
    private GridManager gridManager;

    void Awake()
    {
        rm = ReferenceManager.Instance;
        grid = rm.grid;
        tilemap = rm.groundTileMap;
        movementManager = rm.movementManager;
        gridManager = rm.gridManager;

        cam = Camera.main;
        em = EventManager.GetInstance();
        em.AddListener<CharacterAddedEvent>(OnCharacterAdded);
        em.AddListener<CharacterTurnStartEvent>(OnCharacterTurnStart);
        enabled = false;
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

                Vector3 cellCenter = grid.GetCellCenterWorld(cellCoords);
                targetPosition = new Vector2(cellCenter.x, cellCenter.y + transform.lossyScale.y / 4f);
                moving = true;

                em.Dispatch(new CharacterMovedEvent
                {
                    character = gameObject,
                    gridPos = gridPos
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
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the position of the cube and sphere are approximately equal.
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
            Vector2Int gridCenter = gridManager.GetCenter();
            Debug.Log(gridCenter);
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
                position = transform.position,
            });
        }
        else
        {
            enabled = false;
        }
    }
}
