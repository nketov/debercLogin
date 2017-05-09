using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Login : MonoBehaviour
{

    public InputField email;
    public InputField password;
    public GameObject Preloader;


    // Use this for initialization
    void Start()
    {
        email.text = PlayerPrefs.GetString("email", "");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickLogin()
    {
      
        StartCoroutine(API.Login(email.text, password.text,Preloader));
      
    }



 






}
