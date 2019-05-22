﻿using System;
using System.Text;
using UnityEngine;

[Serializable]
public class IfNode : DrawnNode {
	public override StringBuilder Content => throw new NotImplementedException();

	public override string NodeType { get => "If"; }
	protected override Vector2 WindowSize => new Vector2(100, 100);

	public enum IfSubtype { Method, Equal, NotEqual, LessThan, LessEqual, MoreThan, MoreEqual }
	private IfSubtype currentSubtype;

	public string var1;
	public string var2;
	public DrawnNode boolMethod;
	public string methodArgument;


	public override NodeData GetData() {
		var prev = base.GetData();
		var info = prev.data;
		int lastIndex = NodeData.lastIndex;
		info[lastIndex + 1] = var1;
		info[lastIndex + 2] = var2;
		info[lastIndex + 3] = boolMethod.ID;

		return prev;
	}

	public override void SetData(NodeData data) {
		base.SetData(data);
		var info = data.data;
		int lastIndex = NodeData.lastIndex;
		var1 = (string)info[lastIndex + 1];
		var2 = (string)info[lastIndex + 2];
		boolMethod = GetNodeByID((int)info[lastIndex + 3]);
	}

	private string ComparatorSign
	{
		get
		{
			switch (currentSubtype)
			{
				case IfSubtype.Equal:
					return "==";
				case IfSubtype.NotEqual:
					return "!=";
				case IfSubtype.LessThan:
					return "<";
				case IfSubtype.LessEqual:
					return "<=";
				case IfSubtype.MoreThan:
					return ">";
				case IfSubtype.MoreEqual:
					return ">=";
			}
			return " ";
		}
	}

	private string Condition
	{
		get
		{
			if(currentSubtype != IfSubtype.Method)
				return $"{var1} {ComparatorSign} {var2}";
			//TODO: Nodo Metodo
			throw new NotImplementedException();
		}
	}

}
