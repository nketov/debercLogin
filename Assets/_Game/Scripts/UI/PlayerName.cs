using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerName : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameManager.Instance)
            GetComponent<Text>().text = GameManager.Instance.userid;
        else
            GetComponent<Text>().text = "";
            /*SceneManager.LoadScene ("login");*/
    }
	
	// Update is called once per frame
	void Update () {

	}
}
