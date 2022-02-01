using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IBaseObject
{
    private int playerId;
    private string name;
    private bool isMain;
    private bool isAlive;
    private PlayerResources resources;
    private List<Planet> planets;

    public Player(int playerId, bool isMain) {
        string playerName = Util.GeneratePlayerName();
        this.init(playerId, playerName, isMain);
    }

    public Player(int playerId, string name, bool isMain) {
        this.init(playerId, name, isMain);
    }

    private void init(int playerId, string name, bool isMain) {
        this.playerId = playerId;
        this.name = name;
        this.isMain = isMain;
        this.isAlive = true;
        this.resources = new PlayerResources();
        this.planets = new List<Planet>();
    }

    public int Id() {
        return playerId;
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
