using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableOptions{
    public int rate;
    public NumberPlayer numberPlayer;
    public GameLimit limit;
    public int timeTurn;
    public bool replacingSeven;

    // по-умолчанию
    public TableOptions() {
        this.rate = 100;
        this.numberPlayer = NumberPlayer.PlayACouple;
        this.limit = GameLimit.Score_1001;
        this.timeTurn = 30;
        this.replacingSeven = true;
    }

    public TableOptions(int rate, NumberPlayer numberPlayer, GameLimit limit, int timeTurn, bool replacingSeven)
    {
        this.rate = rate;
        this.numberPlayer = numberPlayer;
        this.limit = limit;
        this.timeTurn = timeTurn;
        this.replacingSeven = replacingSeven;
    }

}
