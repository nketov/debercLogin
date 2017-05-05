using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour {

    public UnityEngine.Audio.AudioMixer audioMixer;

    void Start()
    {
		GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume", -15f);
	}


	public void Change()  {
		var _value = (GetComponent<Slider> ().value < -33f) ? -80f : GetComponent<Slider> ().value;
		audioMixer.SetFloat("MusicVolume", _value);
	}

	public void Save() {
		var _value = (GetComponent<Slider> ().value < -33f) ? -80f : GetComponent<Slider> ().value;
		PlayerPrefs.SetFloat("MusicVolume", _value);
	}

}
