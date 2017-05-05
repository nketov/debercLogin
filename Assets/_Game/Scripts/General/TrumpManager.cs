using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrumpManager : MonoBehaviour
{

    public TrumpSuit[] TrumpSuitsArray;

    public TrumpSuit[] SelectedTrumpSuitsArray;

    public bool trumpFirst = false;

    public Suit suit;

    // Use this for initialization
    void OnEnable()
    {
        foreach (TrumpSuit _suit in TrumpSuitsArray) {
            _suit.gameObject.SetActive(false);
        }

        foreach (TrumpSuit _suit in SelectedTrumpSuitsArray)
        {
            _suit.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void VisibleTrump(string trumpStr)
    {
        if (trumpFirst)
            suit = GetSuit(trumpStr);
        else
            suit = Suit.None;

        foreach (TrumpSuit _suit in TrumpSuitsArray) {
            // Если выбираем в первый раз - показываем присланную масть если второй - показываем другие масти
            if (_suit.suit == GetSuit(trumpStr)) 
                _suit.gameObject.SetActive(trumpFirst);
            else 
                _suit.gameObject.SetActive(!trumpFirst);
        }
    }

    private Suit GetSuit(string suitStr)
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            if (suit.ToString()[0] == suitStr[0])
                return suit;
        return Suit.None;
    }

    public void SetTrump() {
        if (!trumpFirst && suit == Suit.None)
        {
            return;
        }

        GameManager.Instance._connection.Send("setTrump", suit.ToString());
        gameObject.SetActive(false);
    }

    public void PassTrump()
    {
        if (trumpFirst)
            GameManager.Instance._connection.Send("passTrumpFirst");
        else
            GameManager.Instance._connection.Send("passTrumpSecond");

        gameObject.SetActive(false);
    }

    public void SelectedTrumpVisible(string suitStr) {
        Suit _suit = (Suit)Enum.Parse(typeof(Suit), suitStr);

        foreach (TrumpSuit trumpSuit in SelectedTrumpSuitsArray) {
            if (_suit == trumpSuit.suit)
                trumpSuit.gameObject.SetActive(true);
        }
    }

}
