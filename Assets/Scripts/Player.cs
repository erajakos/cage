using System.Collections.Generic;
using UnityEngine;
using Events;

using Characters;

public class Player
{
    public string name;
    public List<Character> characters;
    private EventManager em;
    private int characterTurn = 0;
    private Character currentCharacter;
    private bool hasTurn = false;

    public Player(string name)
    {
        this.name = name;
        characters = new List<Character>();
        AddCharacters();

        em = EventManager.GetInstance();
        em.AddListener<PlayerTurnStartEvent>(OnPlayerTurnStart);
        em.AddListener<CharacterTurnEndEvent>(OnCharacterTurnEnd);
    }

    private void AddCharacters()
    {
        string face = (name == "Erkki") ? "characterA" : "characterB";
        Character character1 = new Character(face);
        Character character2 = new Character(face);
        Character character3 = new Character(face);

        characters.Add(character1);
        characters.Add(character2);
        characters.Add(character3);
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
            Character character = characters[characterTurn];
            currentCharacter = character;
            em.Dispatch(new CharacterTurnStartEvent
            {
                character = character
            });
        }
    }
}
