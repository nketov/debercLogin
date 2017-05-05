using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour {


    public UnityEngine.Audio.AudioMixer audioMixer;


    void Start() {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("SoundVolume", -15f);
    }

    public void Change()  {
		var _value = (GetComponent<Slider> ().value < -40f) ? -80f : GetComponent<Slider> ().value;
        audioMixer.SetFloat("SoundVolume", _value);
    }

    public void Save() {
		var _value = (GetComponent<Slider> ().value < -40f) ? -80f : GetComponent<Slider> ().value;
		PlayerPrefs.SetFloat("SoundVolume", _value);
	}

}
