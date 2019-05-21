using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class MethodNode : DrawnNode
{
	public string methodName;
	public bool isPublic;
	private DrawnNode firstStatement;
	public enum ReturnType { Void, Bool, Int, Float}
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

	public override StringBuilder Content {
		get {
			var current = firstStatement;
			var cont = new StringBuilder();
			while(current != null)
			{
				cont.Append(current.Content);
				current = current.nextNode;
			}
			return cont;
		}
	}


	public override string NodeType => "Method";

	protected override Vector2 WindowSize => new Vector2(100, 100);

	public override NodeData GetData() {
		var prev = base.GetData();
		var info = prev.data;
		int lastIndex = NodeData.lastIndex;

		info[lastIndex + 1] = currentType;
		info[lastIndex + 2] = GetID(firstStatement);

		return prev;
	}

	public override void SetData(NodeData data) {
		base.SetData(data);
		var info = data.data;
		int lastIndex = NodeData.lastIndex;

		currentType = (ReturnType)info[lastIndex + 1];
		firstStatement = GetNodeByID((int)info[lastIndex + 2]);

	}
}
