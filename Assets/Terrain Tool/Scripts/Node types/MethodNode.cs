using System;
using System.Text;
using UnityEngine;
using UnityEditor;

[Serializable]
public class MethodNode : DrawnNode {
	public string methodName;
	public bool isPublic;
	private DrawnNode firstStatement;
	public enum ReturnType { Void, Bool, Int, Float }
	public ReturnType currentType;
	public string MethodReturn {
		get {
			switch(currentType)
			{
				case ReturnType.Void:
					return "void";
				case ReturnType.Bool:
					return "bool";
				case ReturnType.Int:
					return "int";
				case ReturnType.Float:
					return "float";
				default:
					return null;
			}
		}
	}

	public override StringBuilder Content =>
		GetContentUntilEnd(firstStatement);

	public override string NodeType => "Method";

	protected override Vector2 WindowSize => new Vector2(100, 100);

	public override NodeData GetData() {
		var prev = base.GetData();
		var info = prev.data;
		int lastIndex = NodeData.lastIndex;

		info[lastIndex + 0] = currentType;
		info[lastIndex + 1] = GetID(firstStatement);

		return prev;
	}

	public override void SetData(NodeData data) {
		base.SetData(data);
		var info = data.data;
		int lastIndex = NodeData.lastIndex;

		currentType = (ReturnType)info[lastIndex + 0];
		firstStatement = GetNodeByID((int)info[lastIndex + 1]);
	}

	public override void DrawConnections() {
		base.DrawConnections();
		DrawLine(firstStatement, Color.blue);
	}
}
