using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Events;
using System.Linq;

public class PositionManager : MonoBehaviour
{
    Dictionary<Vector3Int, GameObject> characterPositions;
    public Tilemap tilemapGround;
    public GridManager gridManager;

    public bool hasCharacter(Vector3Int pos)
    {
        return characterPositions.ContainsKey(pos);
    }

    private void Start()
    {
        characterPositions = new Dictionary<Vector3Int, GameObject>();

        EventManager em = EventManager.GetInstance();
        em.AddListener<CharacterMovedEvent>(OnCharacterMovedEvent);
    }

    public Vector3Int GetClosestFreeTile(Vector3Int pos)
    {
        if (tilemapGround.HasTile(pos) && !hasCharacter(pos))
        {
            return pos;
        }

        int distance = 0;
        while (distance < Mathf.Max(gridManager.Rows, gridManager.Cols))
        {
            for (int x = -distance - 1; x <= distance + 1; x++)
            {
                for (int y = -distance - 1; y <= distance + 1; y++)
                {
                    Vector3Int gridPos = new Vector3Int(pos.x + x, pos.y + y, 0);
                    if (tilemapGround.HasTile(gridPos) && !hasCharacter(gridPos))
                    {
                        return gridPos;
                    }
                }
            }
            distance++;
        }

        Debug.Log("Couldn't find suitable tile");
        return pos;
    }

    private void OnCharacterMovedEvent(CharacterMovedEvent e)
    {
        try
        {
            Vector3Int key = characterPositions.First(x => x.Value == e.character).Key;
            characterPositions.Remove(key);
        } catch (InvalidOperationException)
        {
            // not in dictionary
        }
        characterPositions[e.gridPos] = e.character;
    }
}