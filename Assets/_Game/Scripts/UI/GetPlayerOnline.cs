using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerOnline : MonoBehaviour {
	Text textOnline;
	float timer;
	public float timeDef;

	// Use this for initialization
	void Start () {
		textOnline = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer = timeDef;
			if (GameManager.Instance && GameManager.Instance._client != null)
					textOnline.text = GameManager.Instance._client.PlayerInsight.PlayersOnline.ToString();
		}
	}
}
