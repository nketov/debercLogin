using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class API {

    public static string serverUrl = "http://deberz.ti.dn.ua/api/index2.php";
    public static Texture2D avatar;

    public static  IEnumerator Login(string email, string password,GameObject Preloader)
    {
        WWW www = new WWW(serverUrl+"?method=login" +
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

        Debug.Log(www.text);

        
        var pars = www.text.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
        var dic = pars.Select(n => n.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(k => k[0], v => v[1]);

        if (dic["result"] == "OK")
        {
            Preloader.SetActive(true);

            www =new WWW(dic["avatar"]);
            yield return www;

            avatar=www.texture;

            PlayerPrefs.SetString("username", dic["name"]);
            PlayerPrefs.SetString("email", email);

            GameManager.Instance.ConnectServer();
            GameManager.Instance.userid = PlayerPrefs.GetString("username", "");
            
        }     
        
    }


    public static IEnumerator SaveAvatar(Texture2D tex, string userId)
    {

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/avatar_images/avatar_"+userId+".png", bytes);

        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddBinaryData("fileUpload", bytes);

        WWW www = new WWW(serverUrl, form);
        yield return www;

        Debug.Log(www.text);

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
        }

        GameObject.Find("ImageMaker").SetActive(false);
   
    }

    

}
