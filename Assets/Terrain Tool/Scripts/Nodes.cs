using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public Rect myRect;
    public string nodeName;
    public string dialogo;
    public float duration;
    private bool _overNode;
    public List<Nodes> connected;

    public Nodes(float x, float y, float width, float height, string name)
    {
        myRect = new Rect(x, y, width, height);
        connected = new List<Nodes>();
        nodeName = name;
    }

    public void CheckMouse(Event cE, Vector2 pan)
    {
        _overNode = myRect.Contains(cE.mousePosition - pan);
    }

    public bool OverNode
    { get { return _overNode; } }

}
