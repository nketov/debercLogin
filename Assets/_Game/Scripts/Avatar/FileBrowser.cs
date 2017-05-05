using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class FileBrowser
{
    //public 
    //Optional Parameters
    public string name = "File Browser"; //Just a name to identify the file browser with
                                         //GUI Options
                                         // public GUISkin guiSkin; 


    public Texture2D fileTexture, directoryTexture, backTexture, driveTexture; //textures used to represent file types
    public GUIStyle backStyle, cancelStyle, selectStyle; //styles used for specific buttons
    public Color selectedColor = new Color(0.5f, 0.5f, 0.9f); //the color of the selected file
    public bool isVisible { get { return visible; } } //check if the file browser is currently visible
                                                      //File Options

    public bool hasAvatar { get { return isAvatar; } }
    public string searchPattern = "*"; //search pattern used to find files
                                           //Output
    public string outputFile; //the selected output file
                              //Search
                              //	public bool showSearch = false; //show the search bar
                              //public bool searchRecursively = false; //search current folder and sub folders
                              //Protected	
                              //GUI
    protected Vector2 fileScroll = Vector2.zero, folderScroll = Vector2.zero, driveScroll = Vector2.zero;
    protected Color defaultColor;
    protected bool isAvatar = false;
    protected Rect guiSize;
    //protected GUISkin oldSkin;
    public bool visible = false;
    protected int layout;
    //Search
    protected string searchBarString = ""; //string used in search bar
    protected bool isSearching = false; //do not show the search bar if searching
                                        //File Information
    protected DirectoryInfo currentDirectory;
    protected FileInformation[] files;
    protected DirectoryInformation[] directories, drives;
    protected DirectoryInformation parentDir;
    protected bool getFiles = true, showDrives = false;
    protected int selectedFile = -1;
  
    //Constructors
    public FileBrowser(string directory, int layoutStyle, Rect guiRect) { currentDirectory = new DirectoryInfo(directory); layout = layoutStyle; guiSize = guiRect; }
    public FileBrowser(string directory, int layoutStyle) : this(directory, layoutStyle, new Rect(0, 0, Screen.width, Screen.height)) { }
    public FileBrowser(string directory) : this(directory, 1) { }
    public FileBrowser(Rect guiRect) : this() { guiSize = guiRect; }
    public FileBrowser(int layoutStyle) : this(Directory.GetCurrentDirectory(), layoutStyle) { }
    public FileBrowser() : this(Directory.GetCurrentDirectory()) { }

    //set variables
    public void setDirectory(string dir) { currentDirectory = new DirectoryInfo(dir); }
    // public void setLayout(int l) { layout = l; }
    public void setGUIRect(Rect r) { guiSize = r; }


    //gui function to be called during OnGUI



    public bool draw()
    {
        if (getFiles)
        {
            getFileList(currentDirectory);
            getFiles = false;
        }
        GUILayout.BeginArea(guiSize);
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        GUILayout.Label(currentDirectory.FullName);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("box");
        GUILayout.BeginVertical(GUILayout.MaxWidth(300));
        folderScroll = GUILayout.BeginScrollView(folderScroll);
        if (showDrives)
        {
            foreach (DirectoryInformation di in drives)
            {
                if (di.button()) { getFileList(di.di); }
            }
        }
        else
        {
            if ((backStyle != null) ? parentDir.button(backStyle) : parentDir.button())
                getFileList(parentDir.di);
        }
        foreach (DirectoryInformation di in directories)
        {
            if (di.button()) { getFileList(di.di); }
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.BeginVertical("box");

        fileScroll = GUILayout.BeginScrollView(fileScroll);
        for (int fi = 0; fi < files.Length; fi++)
        {
            if (files[fi].fi.EndsWith(".png") || files[fi].fi.EndsWith(".jpg")) {
                if (selectedFile == fi)
                {
                    defaultColor = GUI.color;
                    GUI.color = selectedColor;
                }
                if (files[fi].button())
                {
                    outputFile = (files[fi].di + "\\" + files[fi].fi);
                    selectedFile = fi;
                    return true;
                }
                if (selectedFile == fi)
                    GUI.color = defaultColor;
            }
        }
        GUILayout.EndScrollView();


        GUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        if ((cancelStyle == null) ? GUILayout.Button("Cancel", new GUILayoutOption[] { GUILayout.Height(30) }) : GUILayout.Button("Cancel", cancelStyle))
        {
            outputFile = null;
            return true;
        }
        GUILayout.FlexibleSpace();
        if ((selectStyle == null) ? GUILayout.Button("Select", new GUILayoutOption[] { GUILayout.Height(30) }) : GUILayout.Button("Select", selectStyle))
        {

            isAvatar = true;
            outputFile = null;
            return true;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
        return false;
    }



    public void getFileList(DirectoryInfo di)
    {

        try
        {
            //set current directory
            currentDirectory = di;
            //get parent
            if (backTexture)
                parentDir = (di.Parent == null) ? new DirectoryInformation(di, backTexture) : new DirectoryInformation(di.Parent, backTexture);
            else
                parentDir = (di.Parent == null) ? new DirectoryInformation(di) : new DirectoryInformation(di.Parent);
            showDrives = di.Parent == null;

            //get drives
            string[] drvs = System.IO.Directory.GetLogicalDrives();
            drives = new DirectoryInformation[drvs.Length];
            for (int v = 0; v < drvs.Length; v++)
            {
                drives[v] = (driveTexture == null) ? new DirectoryInformation(new DirectoryInfo(drvs[v])) : new DirectoryInformation(new DirectoryInfo(drvs[v]), driveTexture);
            }

            //get directories
            DirectoryInfo[] dia = di.GetDirectories();
            directories = new DirectoryInformation[dia.Length];
            for (int d = 0; d < dia.Length; d++)
            {
                if (directoryTexture)
                    directories[d] = new DirectoryInformation(dia[d], directoryTexture);
                else
                    directories[d] = new DirectoryInformation(dia[d]);
            }

            //get files
            FileInfo[] fia = di.GetFiles(searchPattern); ;
            //FileInfo[] fia = searchDirectory(di,searchPattern);
            files = new FileInformation[fia.Length];
            for (int f = 0; f < fia.Length; f++)
            {
                if (fileTexture)
                    files[f] = new FileInformation(di.FullName, fia[f].Name, fileTexture);
                else
                    files[f] = new FileInformation(di.FullName, fia[f].Name);

            }
        }
        catch (System.UnauthorizedAccessException e)
        {
            Debug.Log("ACCESS EXCEPTION!");
        }
    }




    public float brightness(Color c) { return c.r * .3f + c.g * .59f + c.b * .11f; }

}

public class FileInformation
{
    public string fi;
    public string di;
    public GUIContent gc;

    public FileInformation(string d, string f)
    {
        fi = f;
        di = d;
        gc = new GUIContent(fi);
    }

    public FileInformation(string d, string f, Texture2D img)
    {
        fi = f;
        di = d;
        gc = new GUIContent(fi, img);
    }

    public bool button() { return GUILayout.Button(gc); }
    public void label() { GUILayout.Label(gc); }
    public bool button(GUIStyle gs) { return GUILayout.Button(gc, gs); }
    public void label(GUIStyle gs) { GUILayout.Label(gc, gs); }
}

public class DirectoryInformation
{
    public DirectoryInfo di;
    public GUIContent gc;

    public DirectoryInformation(DirectoryInfo d)
    {
        di = d;
        gc = new GUIContent(d.Name);
    }

    public DirectoryInformation(DirectoryInfo d, Texture2D img)
    {
        di = d;
        gc = new GUIContent(d.Name, img);
    }

    public bool button() { return GUILayout.Button(gc); }
    public void label() { GUILayout.Label(gc); }
    public bool button(GUIStyle gs) { return GUILayout.Button(gc, gs); }
    public void label(GUIStyle gs) { GUILayout.Label(gc, gs); }
}