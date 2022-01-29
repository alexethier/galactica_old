using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    private int playerId;
    private bool isMain;
    private PlayerResources resources;

    public Player(int playerId, bool isMain) {
        this.playerId = playerId;
        this.isMain = isMain;
        this.resources = new PlayerResources();
    }

    public PlayerResources Resources() {
        return resources;
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
}
