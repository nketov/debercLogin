using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTable : MonoBehaviour {

    public GameObject Preloader;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitGameTable() {
        SceneManager.LoadSceneAsync("menu");
        Preloader.SetActive(true);
    }
}
