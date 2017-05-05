using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    public Card[] cardArray = new Card[9];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCards(string name1, string name2, string name3, string name4, string name5, string name6, string name7, string name8, string name9)
    {
        cardArray[0].SetCard(name1);
        cardArray[1].SetCard(name2);
        cardArray[2].SetCard(name3);
        cardArray[3].SetCard(name4);
        cardArray[4].SetCard(name5);
        cardArray[5].SetCard(name6);
        cardArray[6].SetCard(name7);
        cardArray[7].SetCard(name8);
        cardArray[8].SetCard(name9);
        RefreshCards();
    }

    public void RefreshCards()
    {
       /* int countCards = CountCards(true);
        float angleStep = maxAngle * 2 / (CountCards() - 1);
        int i = 0;
        foreach (Card card in cardArray) {
            if (!card.hidden) {
                card.GetComponent<RectTransform>().pivot = new Vector2(0.5f, pivotY);
                card.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, maxAngle - i * angleStep);
                i++;
            }
        }*/
    }

    public int CountCards()
    {
        return cardArray.Length;
    }

    public int CountCards(bool visible)
    {
        int _res = 0;
        foreach (Card card in cardArray) {
            if (card.hidden == !visible)
                _res++; 
        }
        return _res;
    }
}
