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
public class CharacterAddedEvent
{
    public GameObject character;
};
public class CharacterTurnStartEvent
{
    public GameObject character;
};
public class CharacterMovedEvent
{
    public GameObject character;
    public Vector3Int gridPos;
};
public class CharacterTurnEndEvent
{
    public GameObject character;
};
public class CharacterStartPosEvent
{
    public GameObject character;
    public Vector3Int gridPos;
    public Vector3 position;
};
