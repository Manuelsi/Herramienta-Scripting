using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Nodes : MonoBehaviour
{
    public Rect myRect;
    public string nodeName;
    public string dialogo;
    public float duration;
    private bool _overNode;
    public List<Nodes> connected;
    public ConectionPoint inPoint;
    public ConectionPoint outPoint;

    public Nodes(Vector2 position, float width, float height, string name, GUIStyle nodeStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConectionPoint> OnClickInPoint, Action<ConectionPoint> OnClickOutPoint)
    {
        myRect = new Rect(position.x, position.y, width, height);
        connected = new List<Nodes>();
        inPoint = new ConectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        nodeName = name;
    }
    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
    }
    public void CheckMouse(Event cE, Vector2 pan)
    {
        _overNode = myRect.Contains(cE.mousePosition - pan);
    }

    public bool OverNode
    { get { return _overNode; } }

}
