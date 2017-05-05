using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrumpSuit : MonoBehaviour {

    public Suit suit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTrumpSuit() {
        GetComponentInParent<TrumpManager>().suit = suit;
    }
}
