﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Events;

public class MovementManager : MonoBehaviour
{
    public Tilemap tilemapGround;
    public Grid grid;
    private List<GameObject> highlightedTiles;
    private readonly float highlightOffsetY = 0.293f;
    private List<Vector3Int> movementOptions;
    public PositionManager positionManager;

    private void Awake()
    {
        EventManager em = EventManager.GetInstance();
        em.AddListener<CharacterStartPosEvent>(OnCharacterStartPosEvent);
        highlightedTiles = new List<GameObject>();
    }

    public bool IsValidMove(Vector3Int move)
    {
        foreach(Vector3Int mo in movementOptions)
        {
            if (mo == move && !positionManager.hasCharacter(move))
            {
                return true;
            }
        }

        return false;
    }

    private void OnCharacterStartPosEvent(CharacterStartPosEvent e)
    {
        RemoveHighlightedTiles();
        CalculateMovementOptions(e.gridPos, 1);
        if (e.character.tag == "Human")
        {
            ShowHighlightedTiles();
        }
    }

    private void RemoveHighlightedTiles()
    {
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            Destroy(highlightedTiles[i]);
        }

        highlightedTiles = new List<GameObject>();
    }

    private void CalculateMovementOptions(Vector3Int startPos, int maxMoves)
    {
        movementOptions = new List<Vector3Int>();
        for(int x = -maxMoves; x <= maxMoves; x++) {
            for(int y = -maxMoves; y <= maxMoves; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    Vector3Int gridPos = new Vector3Int(startPos.x + x, startPos.y - y, 0);
                    if (tilemapGround.HasTile(gridPos) && !positionManager.hasCharacter(gridPos)) {
                        movementOptions.Add(gridPos);
                    }
                }
            }
        }
    }

    private void ShowHighlightedTiles()
    {
        foreach (Vector3Int mo in movementOptions)
        {
            Vector3 cellCenter = tilemapGround.GetCellCenterWorld(mo);
            GameObject highlightTile = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Highlight"));
            highlightTile.transform.position = new Vector2(cellCenter.x, cellCenter.y + highlightOffsetY);
            highlightedTiles.Add(highlightTile);
        }
    }

    public List<Vector3Int> GetMovementOptions()
    {
        return movementOptions;
    }

}
