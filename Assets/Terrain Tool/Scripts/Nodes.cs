using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class DrawnNode {
	public string nodeName;
	public DrawnNode nextNode;

	public abstract string Content { get; }
	public abstract string NodeType { get; }
	protected abstract Vector2 WindowSize { get; }

	private static int currentAvailableID = 0;
	public int ID { get; protected set; }

	protected Rect _myRect;
	public Rect MyRect
	{
		get => _myRect;
		protected set => _myRect = value;
	}

	public Vector2 RectPos
	{
		get => MyRect.position;
		set => _myRect.position = value;
	}

	public virtual Vector2 ArrowTargetPos
	{
		get => new Vector2(RectPos.x, MyRect.y + (MyRect.height / 2f));
	}

	public virtual Vector2 ArrowSourcePos
	{
		get => new Vector2(RectPos.x + MyRect.width, MyRect.y + (MyRect.height / 2f));
	}

	#region Constructor
	public DrawnNode(Vector2 position) {
		_myRect = new Rect(position, WindowSize);
		GetNewID();
	}

	public DrawnNode() {
		_myRect = new Rect(Vector2.zero, WindowSize);
		GetNewID();
	}

	public DrawnNode(int id) {
		_myRect = new Rect(Vector2.zero, WindowSize);
		SetNodeID(id);
	}
	#endregion

	public bool CheckMouse(Event cE, Vector2 pan) =>
		MyRect.Contains(cE.mousePosition - pan);

	public virtual void DrawLine(DrawnNode target) =>
		Handles.DrawLine(ArrowSourcePos, target.ArrowTargetPos);

	public virtual void DrawConnections() {
		DrawLine(nextNode);
	}

	//public bool OverNode
	//{ get { return _overNode; } }

	public virtual NodeData GetData() {
		var data = new NodeData();
		data.nodeType = NodeType;
		data.id = ID;
		data.data = new List<object> {
			nodeName, //0
            GetNextNodeID()//1
			//Remember to update NodeData LastIndex 
		};
		return data;
	}

	public virtual void SetData(NodeData data) {
		if (NodeType != data.nodeType)
			//throw new ArgumentException($"Node type does not match. Expected {NodeType} but got {data.nodeType}.");
			throw new ArgumentException($"Tipos de nodos no coinciden. Esperaba {NodeType} pero obtuvo {data.nodeType}.");
		ID = data.id;
		var info = data.data;
		nodeName = (string)info[0];
		nextNode = GetNodeByID((int)info[1]);
	}

	public void SetNodeID(int id) =>
		ID = id;

	private int GetNextNodeID() =>
		GetIdByNode(nextNode);

	public static DrawnNode GetNodeByID(int id) {
		throw new NotImplementedException("Falta implementar un metodo que busque la lista de todos los nodos.");
		//Incluir devolver null si es -1
	}

	public static int GetIdByNode(DrawnNode node) =>
		node == null ? -1 : node.ID;

	protected void GetNewID() {
		ID = currentAvailableID;
		currentAvailableID++;
	}

	public static void SetCurrentAssignableID(int id) {
		currentAvailableID = id;
	}
}

[Serializable]
public class NodeData {
	public string nodeType;
	public int id;
	public List<object> data = new List<object>();

	public const int lastIndex = 1;
}
