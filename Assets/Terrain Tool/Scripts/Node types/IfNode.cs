using System;
using System.Text;
using UnityEngine;

[Serializable]
public class IfNode : DrawnNode {
	//Base
	public override string NodeType => "If";
	protected override Vector2 WindowSize => new Vector2(100, 100);

	//Comparator
	public enum IfSubtype { Method, Equal, NotEqual, LessThan, LessEqual, MoreThan, MoreEqual }
	private IfSubtype currentSubtype;

	//Comparison
	public string var1;
	public string var2;
	public MethodNode boolMethod;
	public string methodArgument;

	//Internal nodes
	public DrawnNode firstOfTrue;
	public DrawnNode firstOfFalse;

	public override StringBuilder Content =>
			ScriptAssembler.InsertIf(Condition, GetContentUntilEnd(firstOfTrue), GetContentUntilEnd(firstOfFalse));

	public override NodeData GetData() {
		var prev = base.GetData();
		var info = prev.data;
		int lastIndex = NodeData.lastIndex;
		info[lastIndex + 0] = var1;
		info[lastIndex + 1] = var2;
		info[lastIndex + 2] = boolMethod.ID;

		return prev;
	}

	public override void SetData(NodeData data) {
		base.SetData(data);
		var info = data.data;
		int lastIndex = NodeData.lastIndex;
		var1 = (string)info[lastIndex + 0];
		var2 = (string)info[lastIndex + 1];
		boolMethod = GetNodeByID((int)info[lastIndex + 2]) as MethodNode;
	}

	private string ComparatorSign {
		get {
			switch(currentSubtype)
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

	private string Condition {
		get {
			if(currentSubtype != IfSubtype.Method)
				return $"{var1} {ComparatorSign} {var2}";
			if(boolMethod.currentType != MethodNode.ReturnType.Bool)
			{
				boolMethod = null;
				throw new ArgumentException("If node condition method cannot return anything other than bool");
			}
			return $"{boolMethod.methodName}({(methodArgument != null ? methodArgument : null)})";
		}
	}

	public override void DrawConnections() {
		var rect = MyRect;
		var dividedHeight = (rect.height * 0.9f) / 3f;
		float offset = rect.height * 0.1f;
		DrawLineFrom(new Vector2(rect.width, offset + dividedHeight * 1), boolMethod, Color.blue, true);
		DrawLineFrom(new Vector2(rect.width, offset + dividedHeight * 2), firstOfTrue, Color.green, true);
		DrawLineFrom(new Vector2(rect.width, offset + dividedHeight * 3), firstOfFalse, Color.red, true);
		DrawLineFrom(new Vector2(rect.width / 2f, rect.height), nextNode, true);
	}

}
