using System;
using System.Text;
using UnityEngine;

[Serializable]
public class ForNode : DrawnNode {
	public override StringBuilder Content =>
		ScriptAssembler.InsertFor(counterVariable, iterations, GetContentUntilEnd(firstWithinCycle));

	public override string NodeType => "For";

	protected override Vector2 WindowSize => new Vector2(100, 100);

	public bool fixedCycles = true;
	public string iterations;
	public string counterVariable;

	public DrawnNode firstWithinCycle;


	public override NodeData GetData() {
		var prev = base.GetData();
		var info = prev.data;
		int lastIndex = NodeData.lastIndex;
		info[lastIndex + 0] = fixedCycles;
		info[lastIndex + 1] = iterations;
		info[lastIndex + 2] = counterVariable;
		info[lastIndex + 3] = firstWithinCycle.ID;

		return prev;
	}

	public override void SetData(NodeData data) {
		base.SetData(data);
		var info = data.data;
		int lastIndex = NodeData.lastIndex;

		fixedCycles = (bool)info[lastIndex + 0];
		iterations = (string)info[lastIndex + 1];
		counterVariable = (string)info[lastIndex + 2];
		firstWithinCycle = GetNodeByID((int)info[lastIndex + 3]);
	}

	public override void DrawConnections() {
		base.DrawConnections();
		var rect = MyRect;
		DrawLineFrom(new Vector2(rect.width / 2f, rect.height), firstWithinCycle, Color.blue, true);
	}
}
