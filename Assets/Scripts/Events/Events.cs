using UnityEngine;

public class StartGameEvent { };
public class NextTurnEvent
{
    public int turn;
};
public class PlayerTurnStartEvent {
    public Player player;
};
public class PlayerTurnEndEvent { };
public class CharacterTurnStartEvent
{
    public GameObject character;
};
public class CharacterTurnEndEvent
{
    public GameObject character;
};