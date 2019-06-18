using System;
using System.Text;

[Serializable]
public class DoWhileNode : WhileNode {
	public override string NodeType => "DoWhile";

	public override StringBuilder Content => 
		ScriptAssembler.InsertDoWhile(Condition,GetContentUntilEnd(firstWithinCycle));
}
