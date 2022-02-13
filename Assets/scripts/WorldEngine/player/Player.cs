using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IBaseObject
{
    private int playerId;
    private Color color;
    private string name;
    private bool isMain;
    private bool isAlive;
    private PlayerResources resources;
    private List<Planet> planets;

    public Player(int playerId, Color color, bool isMain) {
        string playerName = Util.GeneratePlayerName();
        this.init(playerId, color, playerName, isMain);
    }

    public Player(int playerId, Color color, string name, bool isMain) {
        this.init(playerId, color, name, isMain);
    }

    private void init(int playerId, Color color, string name, bool isMain) {
        this.playerId = playerId;
        this.color = color;
        this.name = name;
        this.isMain = isMain;
        this.isAlive = true;
        this.resources = new PlayerResources();
        this.planets = new List<Planet>();
    }

    public int Id() {
        return playerId;
    }

    public Color Color() {
        return color;
    }

    public PlayerResources Resources() {
        return resources;
    }

    public bool Alive() {
        return this.isAlive;
    }

    public void NextTurn() {
        if(!isMain) {
            this.TakeTurn();
        }

        this.applyNextTurnResources();
    }

    public string Name() {
        return name;
    }

    private void TakeTurn() {
        Debug.Log(string.Format("Player {0} taking turn.", playerId));
    }

    private void applyNextTurnResources() {
        PlayerResources copy = this.resources.Copy();
        // Apply all calculations to current snapshot of resources and then replace the player's current resource pool.
        this.AdjustIncome(copy);
        this.resources = copy;
    }

    private void AdjustIncome(PlayerResources inputResources) {
        inputResources.AdjustMoney(2);
    }

    public void AddPlanet(Planet planet) {
        this.planets.Add(planet);
    }
}
