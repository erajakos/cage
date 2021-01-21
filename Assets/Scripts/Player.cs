using System.Collections.Generic;
using UnityEngine;
using Events;

public class Player
{
    public string name;
    public List<GameObject> characters;
    private EventManager em;
    private int characterTurn = 0;
    private GameObject currentCharacter;
    private bool hasTurn = false;

    public Player(string name)
    {
        em = EventManager.GetInstance();

        this.name = name;
        characters = new List<GameObject>();
        AddCharacters();

        em.AddListener<PlayerTurnStartEvent>(OnPlayerTurnStart);
        em.AddListener<CharacterTurnEndEvent>(OnCharacterTurnEnd);
    }

    private void AddCharacters()
    {
        string face = (name == "Erkki") ? "characterA" : "characterB";

        GameObject character1 = AddCharacter(face);
        GameObject character2 = AddCharacter(face);
        GameObject character3 = AddCharacter(face);

        characters.Add(character1);
        characters.Add(character2);
        characters.Add(character3);
    }

    private GameObject AddCharacter(string sprite)
    {
        GameObject character = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Character"));
        Sprite characterSprite = Resources.Load<Sprite>("Sprites/Characters/" + sprite);
        character.GetComponent<SpriteRenderer>().sprite = characterSprite;

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
            Debug.Log(name + " has turn now.");
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
            Debug.Log("Character turn: " + characterTurn + " " + name);
            GameObject character = characters[characterTurn];
            currentCharacter = character;
            em.Dispatch(new CharacterTurnStartEvent
            {
                character = character
            });
        }
    }
}
