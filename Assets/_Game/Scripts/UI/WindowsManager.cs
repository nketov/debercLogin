using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WindowsManager : MonoBehaviour {
	public GameObject[] windows;
	public UnityEngine.Audio.AudioMixer audioMixer;

    void Awake()
    {
		audioMixer.SetFloat("SoundVolume", PlayerPrefs.GetFloat("SoundVolume", -15f ));
		audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", -15f ));

		windows [0].SetActive (true);
		for (int i = 1; i < windows.Length; i++) {
			windows [i].SetActive (false);
		}

		if (!GameManager.Instance) {
			SceneManager.LoadScene ("login");
		}  
    }

	public void CreateRoom(){
		if (GameManager.Instance) {
			GameManager.Instance.CreateRoom ();
		}
	}

	public void JoinRoom(){
		if (GameManager.Instance)
			GameManager.Instance.JoinRoom ();
	}

	public void LeaveRoom(){
		if (GameManager.Instance)
			GameManager.Instance.LeaveRoom ();
	}
}

