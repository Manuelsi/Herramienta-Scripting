using System;
using System.Text;
using UnityEngine;

public class WhileNode : DrawnNode {


	//Comparison
	public string var1;
	public string var2;
	public MethodNode boolMethod;
	public string methodArgument;
	private IfNode.ComparisonType conditionType;
	public DrawnNode firstWithinCycle;

	//Base
	public override string NodeType => "While";
	protected override Vector2 WindowSize => new Vector2(100, 100);
	public override StringBuilder Content =>
		ScriptAssembler.InsertWhile(Condition, GetContentUntilEnd(firstWithinCycle));

	private string Condition {
		get {
			if(conditionType != IfNode.ComparisonType.Method)
				return $"{var1} {IfNode.GetComparatorSign(conditionType)} {var2}";

			if(boolMethod.currentType != MethodNode.ReturnType.Bool)
			{
				boolMethod = null;
				throw new ArgumentException("If node condition method cannot return anything other than bool");
			}
			return $"{boolMethod.methodName}({(methodArgument ?? null)})";
		}
	}

	public override void DrawConnections() {
		base.DrawConnections();
		var rect = MyRect;
		DrawLineFrom(new Vector2(rect.width / 2f, rect.height), firstWithinCycle, Color.blue, true);
	}
}
