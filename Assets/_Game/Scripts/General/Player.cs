using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    CardManager cardManager;
    public Text nick;
    public Image arrow;
    public Text score;
    public Text allScore;
    public bool isLocal = false;
    public int indexInArrayPlayers = -1;

    public string userid;

    public string name1;
    public string name2;
    public string name3;
    public string name4;
    public string name5;
    public string name6;
    public string name7;
    public string name8;
    public string name9;

    public bool cardRefresh = false;

    public Sprite Frame;
    public Sprite FrameActive;

    // Use this for initialization
    void OnEnable () {
        TableInit tableInit = GameObject.Find("Canvas").GetComponent<TableInit>();
        cardManager = GetComponentInChildren<CardManager>();
        cardManager.setCards("", "", "", "", "", "", "", "", "");
    }
	
	// Update is called once per frame
	void Update () {
        if (cardRefresh) {
            cardRefresh = false;
            cardManager.setCards(name1, name2, name3, name4, name5, name6, name7, name8, name9);
        }
	}

    public void setNick(string nickName)
    {
        nick.GetComponent<Text>().text = nickName;
        userid = nickName;
    }

    public void setHasTurn(bool state)
    {
        if (state)
            arrow.GetComponent<Image>().sprite = FrameActive;
        else
            arrow.GetComponent<Image>().sprite = Frame;
    }

    public void setCardFromString(string _string) {
        if (_string == "") return;
        string[] Cards = _string.Split(',');
        cardManager.setCards(Cards[0], Cards[1], Cards[2], Cards[3], Cards[4], Cards[5], Cards[6], Cards[7], Cards[8]);
    }
}
