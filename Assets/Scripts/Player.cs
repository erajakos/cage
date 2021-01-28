using System.Collections.Generic;
using UnityEngine;
using Events;

public enum PlayerType
{
    Human,
    Enemy,
    Lemming
}

public class Player
{
    public int playerType;
    public List<GameObject> characters;
    private EventManager em;
    private int characterTurn = 0;
    private GameObject currentCharacter;
    
    private bool hasTurn = false;

    public Player(PlayerType playerType)
    {
        em = EventManager.GetInstance();

        this.playerType = (int)playerType;
        characters = new List<GameObject>();
        AddCharacters();

        em.AddListener<PlayerTurnStartEvent>(OnPlayerTurnStart);
        em.AddListener<CharacterTurnEndEvent>(OnCharacterTurnEnd);
    }

    private void AddCharacters()
    {
        GameObject character1 = AddCharacter();
        GameObject character2 = AddCharacter();
        GameObject character3 = AddCharacter();

        characters.Add(character1);
        characters.Add(character2);
        characters.Add(character3);
    }

    private GameObject AddCharacter()
    {
        string prefab;
        switch ((PlayerType)playerType) {
            case PlayerType.Human:
                prefab = "ChickenBrown";
                break;
            case PlayerType.Lemming:
                prefab = "SheepWhite";
                break;
            case PlayerType.Enemy:
            default:
                prefab = "Character";
                break;
        }

        GameObject character = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/" + prefab));

        em.Dispatch(new CharacterAddedEvent
        {
            character = character
        });

        return character;
    }

    private void OnPlayerTurnStart(PlayerTurnStartEvent e)
    {
        if (ReferenceEquals(e.player, this))
        {
            Debug.Log((PlayerType)e.player.playerType + " has turn now.");
            characterTurn = 0;
            hasTurn = true;

            NewCharacterTurn();
        }
    }

    void OnCharacterTurnEnd(CharacterTurnEndEvent e)
    {
        if (hasTurn && ReferenceEquals(e.character, currentCharacter))
        {
            characterTurn++;
            
            if (characterTurn < characters.Count)
            {
                NewCharacterTurn();
            }
            else
            {
                hasTurn = false;
                em.Dispatch<PlayerTurnEndEvent>();
            }
        }
    }

    void NewCharacterTurn()
    {
        if (hasTurn)
        {
            Debug.Log("Character turn: " + characterTurn + " " + (PlayerType)playerType);
            GameObject character = characters[characterTurn];
            currentCharacter = character;
            em.Dispatch(new CharacterTurnStartEvent
            {
                character = character
            });
        }
    }
}
