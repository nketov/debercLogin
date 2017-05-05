using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class imageBrowser : MonoBehaviour
{

    //skins and textures


    //public GUISkin[] skins;
    public Texture2D file, folder, back, drive;
    protected Texture2D avatarImage = null;
    public Texture2D texNotFound;
    public GameObject ImgMkr;
    public GameObject Preloader;
    [SerializeField]
    private Kakera.Unimgpicker imagePicker;
  //  bool complete;
    float coef;
    //string[] layoutTypes = { "Type 0", "Type 1" };
    //initialize file browser
    FileBrowser fb = new FileBrowser();
    string output = "no file";
    // Use this for initialization
    void OnEnable()
    {
        //complete = false;
#if UNITY_ANDROID
        OnPressShowPicker();

        imagePicker.Completed += (string path) =>
            {     
                StartCoroutine(LoadTexture(path));
             };

        imagePicker.Failed += (string message) =>
        {
            transform.parent.gameObject.SetActive(false);
        };


#else
        //setup file browser style
        //	fb.guiSkin = skins[0]; //set the starting skin
        //set the various textures
        fb.fileTexture = file;
        fb.directoryTexture = folder;
        fb.backTexture = back;
        fb.driveTexture = drive;
        //show the search bar
        //fb.showSearch = true;
        //search recursively (setting recursive search may cause a long delay)
        //fb.searchRecursively = true;
        GetComponent<RawImage>().texture = texNotFound;
#endif
    }

    void OnGUI()
    {
#if UNITY_ANDROID
        Debug.Log("Android");
#else

        GUILayout.BeginHorizontal();
        //GUILayout.BeginVertical();
        //	GUILayout.Label("Layout Type");
        //	fb.setLayout(GUILayout.SelectionGrid(fb.layoutType,layoutTypes,1));
        //GUILayout.Space(10);
        //select from available gui skins
        //	GUILayout.Label("GUISkin");
        //	foreach(GUISkin s in skins){
        //	if(GUILayout.Button(s.name)){
        //			fb.guiSkin = s;
        //		}
        //	}
        //GUILayout.Space(10);
        //fb.showSearch = GUILayout.Toggle(fb.showSearch,"Show Search Bar");
        //fb.searchRecursively = GUILayout.Toggle(fb.searchRecursively,"Search Sub Folders");
        //GUILayout.EndVertical();
        GUILayout.Space(10);
        GUILayout.Label("Selected File: " + output);
        GUILayout.Label("File: " + fb.outputFile);
        GUILayout.EndHorizontal();
        //draw and display output
        if (fb.draw())
        { //true is returned when a file has been selected
          //the output file is a member if the FileInfo class, if cancel was selected the value is null
            output = (fb.outputFile == null) ? "cancel hit" : fb.outputFile;
            if (fb.outputFile != null) { StartCoroutine(LoadTexture(output)); }
            else
            {


                if (fb.hasAvatar && avatarImage != null)
                {
                    PickComplete();
                }
                else
                {
                    GetComponent<RawImage>().texture = texNotFound;
                    GameObject.Find("ImageBrowser").SetActive(false);
                    ImgMkr.SetActive(false);
                }
            }
        }
#endif
    }

    private void PickComplete()
    {
        if ((float)avatarImage.width / 526.0f > (float)avatarImage.height / 305.0f)
        {
            coef = (float)avatarImage.width / 526.0f;
            
        }
        else
        {
            coef = (float)avatarImage.height / 305.0f;
        }

        Preloader.SetActive(true);

        TextureScale.Bilinear(avatarImage, (int)Mathf.Round(avatarImage.width/coef), (int)Mathf.Round(avatarImage.height / coef));
        GameObject.Find("ImageBrowser").SetActive(false);
        Preloader.SetActive(false);
        ImgMkr.SetActive(true);
        GameObject.Find("ImgWnd").GetComponent<RectTransform>().sizeDelta = new Vector2(avatarImage.width, avatarImage.height);
        GameObject.Find("Crop").GetComponent<editImage>().original = avatarImage;
        GameObject.Find("ImgWnd").GetComponent<Image>().sprite = Sprite.Create(avatarImage, new Rect(0.0f, 0.0f, avatarImage.width, avatarImage.height), new Vector2(0.5f, 0.5f), 100.0f);
        GameObject.Find("Crop").GetComponent<editImage>().Renew();
    }

    IEnumerator LoadTexture(string output)
    {
        Preloader.SetActive(true);
        WWW www = new WWW("file://" + output);
      //  complete = true;
        yield return www;

        if (www.error == null && www.texture != null)
        {
            avatarImage = www.texture;
        }
        else
        {
            avatarImage = texNotFound;
            Debug.Log("Texture not found");
        }

#if UNITY_ANDROID
        PickComplete();
#else

        GetComponent<RawImage>().texture = avatarImage;
        Preloader.SetActive(false);
#endif
    }

    public void OnPressShowPicker()
    {
#if UNITY_ANDROID
        imagePicker.Show("Select Image", "unimgpicker", 1024);    
#endif
    }

//    void OnApplicationFocus(bool hasFocus)
//    {
//#if UNITY_ANDROID
//        if (hasFocus)
//            StartCoroutine(CloseThisWindow(0.2f));
//#endif
//    }

//    IEnumerator CloseThisWindow(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        if (!complete)
//            transform.parent.gameObject.SetActive(false);
//    }



}
