using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTableActivate : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		//TODO заполнение по-умолчанию
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetRate(int rate)
    {
        GameManager.Instance.currentTableOptions.rate = rate;
    }

    public void SetLimit(int _limit)
    {
        GameManager.Instance.currentTableOptions.limit = (GameLimit)_limit;
    }

    public void SetNumberPlayer(int _numberPlayer)
    {
        GameManager.Instance.currentTableOptions.numberPlayer = (NumberPlayer)_numberPlayer;
    }

    public void SetTimeTurn(int timeTurn)
    {
        GameManager.Instance.currentTableOptions.timeTurn = timeTurn;
    }

    public void SetReplacingSeven(bool replacingSeven)
    {
        GameManager.Instance.currentTableOptions.replacingSeven = replacingSeven;
    }
}
