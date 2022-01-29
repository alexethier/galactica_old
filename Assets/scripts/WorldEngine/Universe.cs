using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe
{

    private int turnCount;
    private Player mainPlayer;
    private Player[] players;

    private static Universe universe;
    public static Universe Instance()
    {
        if (universe == null)
        {
            //universe = FindObjectOfType(typeof(Universe)) as Universe;
            universe = new Universe();
            universe.Start();
        }
        return universe;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting Game!");

        turnCount = 0;
        int playerCount = 4;
        players = new Player[playerCount];

        for(int i=0; i < players.Length; i++) {
            Player player = null;
            if(i == 0) {
                player = new Player(i, true);
                mainPlayer = player;
            } else {
                player = new Player(i, false);
            }
            players[i] = player;
        }   
    }

    public Player MainPlayer() {
        return mainPlayer;
    }

    public void NextTurn() {
        Debug.Log("Universe: Next Turn Clicked!");
        foreach(Player player in players) {
            player.NextTurn();
        }

        turnCount = turnCount + 1;
    }
}
