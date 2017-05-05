using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImageScript : MonoBehaviour 
{
    public Sprite sprite;

    void OnEnable()
    {
        GetComponent<Image>().sprite = sprite;
        GetComponent<Image>().SetNativeSize();

    }
}
