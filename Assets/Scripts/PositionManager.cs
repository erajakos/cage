using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class PositionManager : MonoBehaviour
{
    Dictionary<Vector3Int, GameObject> characterPositions;

    public bool hasCharacter(Vector3Int pos)
    {
        return characterPositions.ContainsKey(pos);
    }

    private void Awake()
    {
        characterPositions = new Dictionary<Vector3Int, GameObject>();

        EventManager em = EventManager.GetInstance();
        em.AddListener<CharacterMovedEvent>(OnCharacterMovedEvent);
    }

    private void OnCharacterMovedEvent(CharacterMovedEvent e)
    {
        characterPositions[e.gridPos] = e.character;
    }
}