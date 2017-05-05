using UnityEngine;
using UnityEditor;
using System.Collections;

/*
 * Unity Transform Editor Script
 * By Chevy Ray Johnston (happytrash@gmail.com)
 * 
 * Note: This is an editor class. To use it you have to place your script in Assets/Editor inside your project folder.
 * Custom editors run automatically, so once you've placed the script in your project, your transforms should look different.
 */
[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    static bool mode2D;

    public override void OnInspectorGUI()
    {
        //Grab target & modifiable values
        var t = (Transform)target;
        var p = t.localPosition;
        var r = t.localEulerAngles;
        var s = t.localScale;

        //Toggle 2D mode
        mode2D = GUILayout.Toggle(mode2D, "2D Mode");
        
        if (mode2D)
        {
            //Modify 2D transform
            var p2 = EditorGUILayout.Vector2Field("Position", new Vector2(p.x, p.y));
            var s2 = EditorGUILayout.Vector2Field("Scale", new Vector2(s.x, s.y));
            var r2 = EditorGUILayout.Slider("Rotation", r.z, 0, 359);
            p.Set(p2.x, p2.y, p.z);
            s.Set(s2.x, s2.y, s.z);
            r.Set(r.x, r.y, r2);
        }
        else
        {
            //Modify 3D transform
            p = EditorGUILayout.Vector3Field("Position", t.localPosition);
            r = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
            s = EditorGUILayout.Vector3Field("Scale", t.localScale);
        }

        //Reset buttons
        GUILayout.Label("Reset");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Position"))
            p.Set(0, 0, 0);
        if (GUILayout.Button("Rotation"))
            r.Set(0, 0, 0);
        if (GUILayout.Button("Scale"))
            s.Set(1, 1, 1);
        if (GUILayout.Button("All"))
        {
            p.Set(0, 0, 0);
            r.Set(0, 0, 0);
            s.Set(1, 1, 1);
        }
        GUILayout.EndHorizontal();

        //Apply changes
        if (GUI.changed)
        {
            Undo.RegisterUndo(t, "Transform Change");
            t.localPosition = Validate(p);
            t.localEulerAngles = Validate(r);
            t.localScale = Validate(s);
        }
    }

    private Vector3 Validate(Vector3 v)
    {
        if (float.IsNaN(v.x))
            v.x = 0;
        if (float.IsNaN(v.y))
            v.y = 0;
        if (float.IsNaN(v.z))
            v.z = 0;
        return v;
    }
}