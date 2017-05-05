using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    TableInit tableInit;
    public bool hidden = true;
    public Suit suit = Suit.None;

	// Use this for initialization
	void Awake () {
        tableInit = GameObject.Find("Canvas").GetComponent<TableInit>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCard(string name)
    {
        //if (name.Length == 2)
        //    name = name[0] + " " + name[1];

        if (name == "") {
            SetVisibleCard(false);
            return;
        }

        string _name = "";
        if (name.Length == 2 || name.Length == 3)
            _name = "Card_" + name[0] + "-" + name.Substring(1,name.Length - 1);
        else 
            _name = "Card_" + name;

        int i = 0;
        int index = -1;
        foreach (Sprite sprite in tableInit.cardSpriteArray) {
                if (sprite.name == _name)
                    index = i;
                i++;
            }
        if (index > -1)
        {
            GetComponent<Image>().enabled = true;
            GetComponent<Image>().sprite = tableInit.cardSpriteArray[index];
            hidden = false;
        }
        else
        {
            SetVisibleCard(false);
        }
    }

    void SetVisibleCard(bool visible) {
        GetComponent<Image>().enabled = visible;
        hidden = !visible;
    }
}
