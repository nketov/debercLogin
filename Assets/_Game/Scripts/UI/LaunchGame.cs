using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchGame : MonoBehaviour {

    public GameObject Preloader;

	// Use this for initialization
	public void StartGame () {

        SceneManager.LoadSceneAsync("game");
        Preloader.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
