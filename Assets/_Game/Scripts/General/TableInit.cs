using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInit : MonoBehaviour {
    public Sprite[] cardSpriteArray;

	// Use this for initialization
	void Start () {
        if (GameManager.Instance && GameManager.Instance._connection != null) {
            GameManager.Instance._connection.Send("startGame");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
