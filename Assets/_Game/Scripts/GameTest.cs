using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerIOClient;

public class GameTest : MonoBehaviour {
    bool flagStartConnect = false;

    // Use this for initialization
    void Start () {
        GameManager.Instance.ConnectServer();
	}
	
	// Update is called once per frame
	void Update () {
        if (!flagStartConnect && GameManager.Instance._client != null)
        {
            flagStartConnect = true;
            GameManager.Instance.ConnectRoom();
        }
        else
        {
            if (GameManager.Instance._connection != null && GameManager.Instance._connection.Connected) {

            }
        }
	}
}
