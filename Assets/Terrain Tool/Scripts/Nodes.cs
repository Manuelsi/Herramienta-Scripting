using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class DrawnNode
{
    public string nodeName;
	public DrawnNode nextNode;

	public abstract string Content { get; protected set; }
	public abstract string NodeType { get; protected set; }
	protected abstract Vector2 WindowSize { get; }

	protected Rect _myRect;
    public Rect MyRect {
		get => _myRect;
		protected set => _myRect = value;
	}

	public Vector2 RectPos {
		get => MyRect.position;
		set => _myRect.position = value ;
	}

	public virtual Vector2 ArrowTargetPos {
		get => new Vector2(RectPos.x, MyRect.y + (MyRect.height / 2f));
	}

	public virtual Vector2 ArrowSourcePos {
		get => new Vector2(RectPos.x + MyRect.width, MyRect.y + (MyRect.height / 2f));
	}

    public DrawnNode(Vector2 position)
    {
		_myRect = new Rect(position, WindowSize);
    }

	public DrawnNode() {
		_myRect = new Rect(0, 0, WindowSize.x, WindowSize.y);
	}

    public bool CheckMouse(Event cE, Vector2 pan)=>
        MyRect.Contains(cE.mousePosition - pan);
    
	public virtual void DrawLine(DrawnNode target) =>
		Handles.DrawLine(ArrowSourcePos, target.ArrowTargetPos);

    //public bool OverNode
    //{ get { return _overNode; } }

	public virtual NodeData GetData() {
		var data = new NodeData();
		data.nodeType = NodeType;
		data.data = new List<object> {
			nodeName, //0
			Content //1
		};
		return data;
	}

	public virtual void SetData(NodeData data) {
		if(NodeType != data.nodeType)
			//throw new ArgumentException($"Node type does not match. Expected {NodeType} but got {data.nodeType}.");
			throw new ArgumentException($"Tipos de nodos no coinciden. Esperaba {NodeType} pero obtuvo {data.nodeType}.");
		var info = data.data;
		nodeName = (string)info[0];
		Content = (string)info[1];
	}
}

[Serializable]
public struct NodeData {
	public string nodeType;
	public List<object> data;
}
