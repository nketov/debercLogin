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
        GameObject.Find("Avatar").GetComponent<Image>().sprite = Sprite.Create(API.avatar, new Rect(0.0f, 0.0f, 256, 256), new Vector2(0.5f, 0.5f), 100.0f);
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

