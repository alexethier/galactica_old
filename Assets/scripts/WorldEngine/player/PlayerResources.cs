using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // The namespace for the UI stuff.
using TMPro;

public class PlayerResources
{
    private int money;

    public void AdjustMoney(int adjustment) {
        money = money + adjustment;
    }

    public int Money() {
        return money;
    }

    public PlayerResources Copy() {
        PlayerResources copy = new PlayerResources();
        copy.AdjustMoney(this.money);
        return copy;
    }

}
