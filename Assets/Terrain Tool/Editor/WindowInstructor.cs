using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WindowInstructor : EditorWindow
{
    public GUIStyle myStyle;
    private Vector2 _scrollPosition;
    public GUIStyle myStyle2;

    public static void OpenWindowInstructor()
    {
        var WI = (WindowInstructor)GetWindow(typeof(WindowInstructor));
        WI.myStyle = new GUIStyle();
        WI.myStyle2 = new GUIStyle();
        WI.myStyle.alignment = TextAnchor.MiddleCenter;
        WI.myStyle.fontStyle = FontStyle.Bold;
        WI.myStyle.fontSize = 20;
        WI.myStyle2.alignment = TextAnchor.MiddleCenter;
        WI.myStyle2.fontSize = 15;
        WI.myStyle2.fontStyle = FontStyle.BoldAndItalic;
        WI.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Instruccion de Funcionamiento de Easy Script", myStyle);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        TextInstructor();
    }


    void TextInstructor()
    {
        EditorGUILayout.BeginVertical(GUILayout.Height(110));
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);
        GUILayout.Label(" Elija el tipo de script que quiere utilizar y coloquele su nombre\n Cuando ya tiene creado su Script puede unirlo con el que sigue",myStyle2);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
