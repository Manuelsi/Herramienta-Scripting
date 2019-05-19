﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class WindowsScripts : EditorWindow
{
    private List<Nodes> allNodes;
    private List<Connection> connections;
    private float toolbarHeight = 100;
    private string currentName;
    private bool _panningScreen;


    private Nodes _selectedNode;
    private Rect graphRect;
    private Vector2 graphPan;
    private Vector2 _originalMousePosition;
    private Vector2 prevPan;
    private GUIStyle myStyle;
    private GUIStyle nodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;
    public GUIStyle wrapTextFieldStyle;

    private ConectionPoint selectedinpoint;
    private ConectionPoint selectedOutPoint;

    [MenuItem("CustomTools/ Easy Script")]
    public static void OpenWindows()
    {
        var mySelf = GetWindow<WindowsScripts>();
        mySelf.allNodes = new List<Nodes>();

        mySelf.myStyle = new GUIStyle();
        mySelf.myStyle.fontSize = 20;
        mySelf.myStyle.alignment = TextAnchor.MiddleCenter;
        mySelf.myStyle.fontStyle = FontStyle.BoldAndItalic;

        mySelf.graphPan = new Vector2(0, mySelf.toolbarHeight);
        mySelf.graphRect = new Rect(0, mySelf.toolbarHeight, 1000000, 1000000);

        
        mySelf.wrapTextFieldStyle = new GUIStyle(EditorStyles.textField);
        mySelf.wrapTextFieldStyle.wordWrap = true;

        var nStyle = new GUIStyle();
        
    }
    
    private void OnGUI()
    {
        CheckMouseInput(Event.current);

        EditorGUILayout.BeginVertical(GUILayout.Height(100));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Script Easy", myStyle, GUILayout.Height(50));
        if (GUILayout.Button("Instruction", GUILayout.Width(75), GUILayout.Height(15)))
            WindowInstructor.OpenWindowInstructor();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        currentName = EditorGUILayout.TextField("Nombre: ", currentName);
        EditorGUILayout.Space();
        if (GUILayout.Button("Create Script", GUILayout.Width(100), GUILayout.Height(30)))
            AddNode();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        DrawConnections();
        
        graphRect.x = graphPan.x;
        graphRect.y = graphPan.y;
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.gray);

        
        GUI.BeginGroup(graphRect);

        BeginWindows();
        var oriCol = GUI.backgroundColor;
        for (int i = 0; i < allNodes.Count; i++)
        {
            foreach (var c in allNodes[i].connected)
                Handles.DrawLine(new Vector2(allNodes[i].myRect.position.x + allNodes[i].myRect.width / 2f, allNodes[i].myRect.position.y + allNodes[i].myRect.height / 2f), new Vector2(c.myRect.position.x + c.myRect.width / 2f, c.myRect.position.y + c.myRect.height / 2f));
        }

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] == _selectedNode)
                GUI.backgroundColor = Color.gray;

            allNodes[i].myRect = GUI.Window(i, allNodes[i].myRect, DrawNode, allNodes[i].nodeName);
            GUI.backgroundColor = oriCol;
        }
        EndWindows();
        GUI.EndGroup();
    }
    private void CheckMouseInput(Event currentE)
    {
        if (!graphRect.Contains(currentE.mousePosition) || !(focusedWindow == this || mouseOverWindow == this))
            return;


        if (currentE.button == 2 && currentE.type == EventType.MouseDown)
        {
            _panningScreen = true;
            prevPan = new Vector2(graphPan.x, graphPan.y);
            _originalMousePosition = currentE.mousePosition;
        }
        else if (currentE.button == 2 && currentE.type == EventType.MouseUp)
            _panningScreen = false;

        if (_panningScreen)
        {
            var newX = prevPan.x + currentE.mousePosition.x - _originalMousePosition.x;
            graphPan.x = newX > 0 ? 0 : newX;

            var newY = prevPan.y + currentE.mousePosition.y - _originalMousePosition.y;
            graphPan.y = newY > toolbarHeight ? toolbarHeight : newY;

            Repaint();
        }

        Nodes overNode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].CheckMouse(Event.current, graphPan);
            if (allNodes[i].OverNode)
                overNode = allNodes[i];
        }

        var prevSel = _selectedNode;
        if (currentE.button == 0 && currentE.type == EventType.MouseDown)
        {
            if (overNode != null)
                _selectedNode = overNode;
            else
                _selectedNode = null;

            if (prevSel != _selectedNode)
                Repaint();
        }
    }
    
    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
                connections[i].Draw();
        }
    }
    

    private void AddNode()
    {
        allNodes.Add(new Nodes(_originalMousePosition, 200, 150, currentName,nodeStyle,inPointStyle,outPointStyle,OnClickInPoint,OnClickOutPoint));
        currentName = "";
        
        Repaint();
    }

    private void OnClickInPoint(ConectionPoint inPoint)
    {
        selectedinpoint = inPoint;
        if (selectedinpoint != null)
        {
            if (selectedOutPoint.node != selectedinpoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    private void OnClickOutPoint(ConectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedinpoint != null)
        {
            if (selectedOutPoint.node != selectedinpoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    private void DrawNode(int id)
    {
        
        if (!_panningScreen)
        {

            GUI.DragWindow();

            if (!allNodes[id].OverNode) return;


            if (allNodes[id].myRect.x < 0)
                allNodes[id].myRect.x = 0;

            if (allNodes[id].myRect.y < toolbarHeight - graphPan.y)
                allNodes[id].myRect.y = toolbarHeight - graphPan.y;
        }

    }
    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
            connections = new List<Connection>();
        connections.Add(new Connection(selectedinpoint, selectedOutPoint, OnClickRemoveConnection));
    }
    private void ClearConnectionSelection()
    {
        selectedinpoint = null;
        selectedOutPoint = null;
    }
}
