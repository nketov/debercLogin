#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class IncreaseBundleVersion: EditorWindow
{
	[MenuItem("Build/Current Bundle Version", false, 600)]
	private static void CurrentBundleVersion() {
		Debug.Log ("Current version: " + PlayerSettings.bundleVersion);
	}

	[MenuItem("Build/Increase Bundle Version", false, 800)]
	private static void BundleVersionPP() {

		int incrementUpAt = 999; //if this is set to 9, then 1.0.9 will become 1.1.0
		int incrementUpAtminorVersion = 9; //if this is set to 9, then 1.0.9 will become 1.1.0
		//string versionTextFileNameAndPath = "Assets/Resources/version.txt";

		string versionText = PlayerSettings.bundleVersion;
		if ( string.IsNullOrEmpty( versionText ) ) {
		    versionText = "0.0.1";
		} else {
		    versionText = versionText.Trim(); //clean up whitespace if necessary
		    string[] lines = versionText.Split('.');
		   
		    int majorVersion = 0;
		    int minorVersion = 0;
		    int subMinorVersion = 0;
		
		    if( lines.Length > 0 ) majorVersion = int.Parse( lines[0] );
		    if( lines.Length > 1 ) minorVersion = int.Parse( lines[1] );
		    if( lines.Length > 2 ) subMinorVersion = int.Parse( lines[2] );
		
		    subMinorVersion++;
		    if( subMinorVersion > incrementUpAt ) {
		        minorVersion++;
		        subMinorVersion = 0;
		    }
			if( minorVersion > incrementUpAtminorVersion ) {
		        majorVersion++;
		        minorVersion = 0;
		    }
		
		    versionText = majorVersion.ToString("0") + "." + minorVersion.ToString("0") + "." +    subMinorVersion.ToString("0");
			//save the file (overwrite the original) with the new version number
			//CommonUtils.WriteTextFile(versionTextFileNameAndPath, versionText);
			//save the file to the Resources directory so it can be used by Game code
			System.IO.File.WriteAllText("Assets/Resources/version.txt", versionText);
			AssetDatabase.Refresh();
		   
		}
		Debug.Log( "Version Incremented to " + versionText );
		PlayerSettings.bundleVersion = versionText;
	}
}
#endif