using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ordering.LevelQuestion))]
public class LevelQuestion_Editor :Editor
{
    Ordering.LevelQuestion obj;
    private void OnEnable()
    {
        obj = (Ordering.LevelQuestion)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        GUI.color = Color.green;
        if (GUILayout.Button("Get Options"))
        {
            obj.GetOptions();
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();

    }
}
