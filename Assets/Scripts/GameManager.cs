using System.Collections.Generic;
using UnityEngine;
using Events;

public class GameManager : MonoBehaviour
{
    private List<Player> players;
    private EventManager em;
    private int turn = 0;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        AddPlayer(new Player(PlayerType.Human));
        AddPlayer(new Player(PlayerType.Lemming));

        em = EventManager.GetInstance();
        em.AddListener<PlayerTurnEndEvent>(OnPlayerTurnEnd);

        em.Dispatch<StartGameEvent>();

        Player player = getCurrentPlayer();
        em.Dispatch(new PlayerTurnStartEvent
        {
            player = player
        });
    }

    void AddPlayer(Player player)
    {
        players.Add(player);
    }

    void OnPlayerTurnEnd(PlayerTurnEndEvent e = null)
    {
        turn++;
        em.Dispatch(new NextTurnEvent
        {
            turn = turn
        });

        Player player = getCurrentPlayer();

        em.Dispatch(new PlayerTurnStartEvent
        {
            player = player
        });
    }

    Player getCurrentPlayer()
    {
        int playerIndex = turn % players.Count;
        return players[playerIndex];
    }
}
