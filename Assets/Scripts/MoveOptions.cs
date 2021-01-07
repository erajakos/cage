﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Events;

public class MoveOptions : MonoBehaviour
{
    public Tilemap tilemapGround;
    public Grid grid;
    public List<GameObject> highlightedTiles;
    private float highlightOffsetY = 0.293f;

    private void Awake()
    {
        EventManager em = EventManager.GetInstance();
        em.AddListener<CharacterStartPosEvent>(OnCharacterStartPosEvent);
    }

    private void OnCharacterStartPosEvent(CharacterStartPosEvent e)
    {
        RemoveHighlightedTiles();
        AddHighlightedTiles(e.gridPos, 1);
    }

    private void RemoveHighlightedTiles()
    {
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            Destroy(highlightedTiles[i]);
        }

        highlightedTiles = new List<GameObject>();
    }

    private void AddHighlightedTiles(Vector3Int startPos, int maxMoves)
    {
        List<Vector3Int> movementOptions = new List<Vector3Int>();
        for (int i = 0; i < maxMoves; i++)
        {
            for(int x = -1; x <= 1; x++) {
                for(int y = -1; y <= 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        Vector3Int gridPos = new Vector3Int(startPos.x + x, startPos.y - y, 0);
                        if (tilemapGround.HasTile(gridPos)) {
                            movementOptions.Add(gridPos);
                        }
                    }
                }
            }
        }

        foreach(Vector3Int mo in movementOptions)
        {
            Vector3 cellCenter = tilemapGround.GetCellCenterWorld(mo);
            GameObject highlightTile = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Highlight"));
            highlightTile.transform.position = new Vector2(cellCenter.x, cellCenter.y + highlightOffsetY);
            highlightedTiles.Add(highlightTile);
        }
    }
    
}
