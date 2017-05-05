using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetStateInfo : MonoBehaviour {
	Text infoText;
	// Use this for initialization
	void Start () {
		infoText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance)
			infoText.text = GameManager.Instance.infomsg;
	}
}
