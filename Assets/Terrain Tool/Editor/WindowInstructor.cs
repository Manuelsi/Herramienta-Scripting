using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WindowInstructor : EditorWindow
{
    public GUIStyle myStyle;

    public static void OpenWindowInstructor()
    {
        var WI = (WindowInstructor)GetWindow(typeof(WindowInstructor));
        WI.myStyle = new GUIStyle();
        WI.myStyle.alignment = TextAnchor.MiddleCenter;
        WI.myStyle.fontStyle = FontStyle.Bold;
        WI.myStyle.fontSize = 20;
        WI.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Instruccion de Funcionamiento de Easy Script", myStyle);
    }
}
