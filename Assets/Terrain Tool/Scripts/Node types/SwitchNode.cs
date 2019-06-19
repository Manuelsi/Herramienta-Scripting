using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SwitchNode : DrawnNode {

	public string caseVariable;
	public List<string> cases;
	public List<DrawnNode> firstsOfCases;
	public DrawnNode firstOfDefault;

	public override string NodeType => "Switch";
	protected override Vector2 WindowSize => new Vector2(100, 100);

	public override StringBuilder Content {
		get {
			if(firstOfDefault == null)
				return ScriptAssembler.InsertSwitch(caseVariable, GetCasesArray());
			//else
			return ScriptAssembler.InsertSwitch(caseVariable, GetCasesArray(), GetContentUntilEnd(firstOfDefault));
		}
	}

	private (string, StringBuilder)[] GetCasesArray() {
		int c = cases.Count;
		var array = new (string, StringBuilder)[c];
		for(int i = 0; i < c; i++)
		{
			array[i] = (cases[i], GetContentUntilEnd(firstsOfCases[i]));
		}
		return array;
	}

	public override NodeData GetData() {
		var prev = base.GetData();
		var info = prev.data;
		int lastIndex = NodeData.lastIndex;

		info[lastIndex + 0] = caseVariable;
		info[lastIndex + 1] = cases;
		info[lastIndex + 2] = firstsOfCases.Select(x => x.ID);
		info[lastIndex + 3] = firstOfDefault.ID;
		return prev;
	}

	public override void SetData(NodeData data) {
		base.SetData(data);
		var info = data.data;
		int lastIndex = NodeData.lastIndex;

		caseVariable = (string)info[lastIndex + 0];
		cases = (List<string>)info[lastIndex + 1];
		firstsOfCases = ((List<int>)info[lastIndex + 2]).Select(x => GetNodeByID(x)).ToList();
		firstOfDefault = GetNodeByID((int)info[lastIndex + 3]);
	}

}