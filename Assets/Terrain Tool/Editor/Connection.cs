using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class Connection 
{
    public ConectionPoint inPoint;
    public ConectionPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConectionPoint inPoint,ConectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        Handles.DrawLine(inPoint.rect.center, outPoint.rect.center);
        //Handles.DrawBezier(inPoint.rect.center, outPoint.rect.center, inPoint.rect.center + Vector2.left * 50f, outPoint.rect.center - Vector2.left * 50f, Color.white, null, 2f);
        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 2.5f, Quaternion.identity, 10, 20, Handles.rec))
        {
            if (OnClickRemoveConnection != null)
                OnClickRemoveConnection(this);
        }
    }
}
