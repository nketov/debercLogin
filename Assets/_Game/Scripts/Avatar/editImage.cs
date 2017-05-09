using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;


public class editImage : MonoBehaviour
{

    public Texture2D original;

    private float horizontalSpeed = 10.0F;
    private float verticalSpeed = 10.0F;

    private bool isMove = false;
    private bool isHover = false;

    private int x, y, s;
  

    // Use this for initialization
    void Start()
    {
        Renew();
    }


    public void Hover(bool b)
    {
        isHover = b;
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isMove = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMove = false;
        }


        if (isMove && isHover)
        {

            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");

            if (inBorder(h, v))
            {
                transform.localPosition = new Vector2(transform.localPosition.x + h, transform.localPosition.y + v);


            }
        }

    }

    private bool inBorder(float h, float v)
    {

        if (
                ((transform.localPosition.x + h) > - s/16) &&
                ((transform.localPosition.x + s * 6 / 7 + h) < original.width) &&

                ((transform.localPosition.y + v) > - s / 7 ) &&
                ((transform.localPosition.y + s* 16 / 17 + v) < original.height)

            )
            return true;
        else
            return false;

    }

    public void Crop()
    {

        Texture2D tex = original;

        x = (int)transform.localPosition.x;
        y = (int)transform.localPosition.y;

        Texture2D result = new Texture2D(s * 95 / 119, s * 95 / 119);
        Color[] colors = tex.GetPixels(x+s/16, y+s/7, s * 95 / 119, s * 95 / 119);
        result.SetPixels(colors);
        result.Apply();

        TextureScale.Bilinear(result, 256, 256);
        StartCoroutine(API.SaveAvatar(result, "2"));
      

        //GameObject.Find("Crop").GetComponent<editImage>().original = avatarImage;

        GameObject.Find("Avatar").GetComponent<Image>().sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, 256, 256), new Vector2(0.5f, 0.5f), 100.0f);
        //SaveTextureToFile(result);

    }
    /*
    void SaveTextureToFile(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.persistentDataPath + "/SavedScreen.png", bytes);
    }
    */
    public void Plus()
    {

        if (inBorder(5, 5))
        {
            s += 5;
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(s, s);
        }

    }

    public void Minus()
    {
        if (s > 50)
        {
            s -= 5;
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(s, s);
            
        }

    }

    internal void Renew()
    {
        int deltaX=0,deltaY=0,sizeC=256;

        if (original.width < 526) deltaX = (526 - original.width) / 2;
        if (original.height < 305) deltaY = (305 - original.height) / 2;
        if (original.width < 256) sizeC = original.width-10;
        if (original.height < 256) sizeC = original.height - 10;

        transform.parent.localPosition = new Vector2(deltaX-526/2,deltaY-305/2);

        transform.localPosition = new Vector2(original.width/2-sizeC/2, original.height/2-sizeC/2);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeC, sizeC);

        s = (int)transform.GetComponent<RectTransform>().rect.height;
     
    }
}
