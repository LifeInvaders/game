using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(NightMode))]
public class NightModeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NightMode myTarget = (NightMode) target;
        DrawDefaultInspector();
        // myTarget.name = EditorGUILayout.TextField("name", myTarget.name);
        // EditorGUILayout.LabelField("Level", myTarget._lights.Length.ToString());
        
        if (GUILayout.Button("Change Mode"))
        {
            myTarget.FindLights();
            myTarget.ChangeMode();
        }
    }
}