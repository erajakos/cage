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

    public GameObject FindNearestCharacter(Vector3 currentPosition, string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearest = gos
            .OrderBy(t => Vector3.Distance(currentPosition, t.transform.position))
            .FirstOrDefault();

        return nearest;
    }

    private void Start()
    {
        characterPositions = new Dictionary<Vector3Int, GameObject>();

        EventManager em = EventManager.GetInstance();
        em.AddListener<CharacterMovedEvent>(OnCharacterMovedEvent);
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