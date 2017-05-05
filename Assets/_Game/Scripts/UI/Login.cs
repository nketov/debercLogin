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
    protected String name;
    protected bool isLogin = false;

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
        //		if (username.text == "")
        //			return;


        StartCoroutine(DBLogin(email.text, password.text));

        if (isLogin == false)
            return;

        
       PlayerPrefs.SetString("username", name);
       PlayerPrefs.SetString("email", email.text);

      GameManager.Instance.ConnectServer ();
      GameManager.Instance.userid = PlayerPrefs.GetString("username", "");

      Preloader.SetActive(true);
    }



    public IEnumerator DBLogin(string email, string password)
    {
        WWW www = new WWW("http://deberz/api/index2.php?method=login" +
        "&email=" + WWW.EscapeURL(email) +
        "&password=" + WWW.EscapeURL(password));

        Debug.Log(www.url);

        if (!www.isDone)
            yield return www;

        if (!string.IsNullOrEmpty(www.error))
            yield break;

        if (string.IsNullOrEmpty(www.text))
        {
            Debug.Log("www text empty");
            yield break;
        }

        var pars = www.text.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        var dic = pars.Select(n => n.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(k => k[0], v => v[1]);

        if (dic["result"] == "OK")
        {
            name = dic["name"];
            isLogin = true;
        }

        Debug.Log(www.text);


    }






}
