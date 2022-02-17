using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Universe
{

    private int turnCount;
    private Player mainPlayer;
    private List<Player> players;
    private List<Planet> planets;

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

        GalaxyBuilder galaxyBuilder = new GalaxyBuilder();
        galaxyBuilder.Build(20);

        players = galaxyBuilder.Players();
        planets = galaxyBuilder.Planets();
        mainPlayer = galaxyBuilder.MainPlayer();

        turnCount = 0;
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
