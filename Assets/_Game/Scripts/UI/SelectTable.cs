using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTable : MonoBehaviour {
    GameObject gameListWnd;
    GameObject waitingLaunchWnd;

    // Use this for initialization
    void OnEnable () {
        gameListWnd = GameObject.Find("GameListWnd");
        waitingLaunchWnd = transform.root.Find("WaitingLaunchPopUp").gameObject;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void JoinTable() {
		transform.root.GetComponent<WindowsManager> ().JoinRoom ();
        //gameListWnd.SetActive(false);
        waitingLaunchWnd.SetActive(true);
    }
}
