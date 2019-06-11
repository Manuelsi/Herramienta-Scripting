using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using CustomMSLibrary.Core;
using CustomMSLibrary.Unity;

public static class ScriptAssembler {

	#region "Constants"
	private static string O_BRACE = "{\r\n";
	private static string C_BRACE = "\r\n}";
	public readonly static string NL;

	private const string EMPTY = "";
	#endregion

	static ScriptAssembler() {
		NL = Environment.NewLine;
		O_BRACE = '{' + NL;
		C_BRACE = NL + '}';
	}

	#region Enclose
	public static StringBuilder Ref_Enclose(StringBuilder target, string precedingText, string innerContent) {
		target.Append(precedingText)
			.Append(O_BRACE)
			.Append(innerContent)
			.Append(C_BRACE)
			.Append(NL);
		return target;
	}

	public static StringBuilder Ref_Enclose(StringBuilder target, string precedingText, StringBuilder innerContent) {
		target.Append(precedingText)
			.Append(O_BRACE)
			.Append(innerContent)
			.Append(C_BRACE)
			.Append(NL);
		return target;
	}


	public static StringBuilder Enclose(string precedingText, string innerContent) {
		var newItem = new StringBuilder();
		Ref_Enclose(newItem, precedingText, innerContent);
		return newItem;
	}

	public static StringBuilder Enclose(StringBuilder precedingText, StringBuilder innerContent) =>
		Enclose(precedingText.ToString(), innerContent.ToString());

	public static StringBuilder Enclose(StringBuilder precedingText, string innerContent) =>
		Enclose(precedingText.ToString(), innerContent);

	public static StringBuilder Enclose(string precedingText, StringBuilder innerContent) =>
		Enclose(precedingText, innerContent.ToString());
	#endregion

	#region InsertIf
	//ref
	//string
	public static StringBuilder Ref_InsertIf(StringBuilder target, string condition, string trueContent,
		string elseContent = EMPTY) {

		Ref_Enclose(target, $"if({condition})", trueContent);
		if(elseContent != null && elseContent != EMPTY)
			Ref_Enclose(target, "else", elseContent);
		return target;
	}
	//stringBuilder
	public static StringBuilder Ref_InsertIf(StringBuilder target, string condition, StringBuilder trueContent,
		StringBuilder elseContent = null) {

		Ref_Enclose(target, $"if({condition})", trueContent);
		if(elseContent != null)
			Ref_Enclose(target, "else", elseContent);
		return target;
	}

	//noRef
	//string
	public static StringBuilder InsertIf(string condition, string trueContent, string elseContent = EMPTY) {
		var newString = Enclose(
			new StringBuilder("if(").Append(condition).Append(')'),
			trueContent);
		if(elseContent != null && elseContent != EMPTY)
			newString = Enclose(newString.Append("else"), elseContent);
		return newString;
	}
	//stringBuilder
	public static StringBuilder InsertIf(string condition, StringBuilder trueContent, StringBuilder elseContent = null) {
		var newString = Enclose(
			new StringBuilder("if(").Append(condition).Append(')'),
			trueContent);
		if(elseContent != null)
			newString = Enclose(newString.Append("else"), elseContent);
		return newString;
	}
	#endregion

	#region InsertFor
	public static StringBuilder InsertFor(string counter, string count, string body) =>
	Enclose($"for(int {counter} = 0; {counter} < {count}; {counter}++)", body);

	public static StringBuilder InsertFor(string counter, int count, string body) =>
		InsertFor(counter, count.ToString(), body);

	public static StringBuilder InsertFor(string counter, string count, StringBuilder body) =>
		Enclose($"for(int {counter} = 0; {counter} < {count}; {counter}++)", body);
	#endregion


	#region InsertWhile
	public static StringBuilder InsertWhile(string condition, StringBuilder body) {
		return Enclose(condition, body);
	} 

	#endregion

	public static string WrapStatement(params string[] items) {
		if(items == null || items.Length == 0)
			return null;
		int c = items.Length;
		string newLine = "";
		for(int i = 0; i < c; i++)
		{
			newLine += items[i] + " ";
		}
		newLine += ';';
		return newLine;
	}

	#region Form Methods
	public static StringBuilder Ref_FormMethod(StringBuilder target, string name,
		StringBuilder body, bool isPublic = false, string type = "void") =>
		Ref_Enclose(target, $"{(isPublic ? "public" : "private")} {type} {name} ()", body);

	public static StringBuilder FormMethod(string name, StringBuilder body, bool isPublic = false, string type = "void") =>
		Enclose($"{(isPublic ? "public" : "private")} {type} {name} ()", body);

	#endregion

	#region Merge
	public static StringBuilder Ref_Merge(StringBuilder target, params StringBuilder[] lines) {
		if(lines == null)
			return target;
		int c = lines.Length;
		if(c == 0)
			return target;
		for(int i = 0; i < c; i++)
		{
			if(lines[i] == null)
				continue;
			target.Append(lines[i]).Append(NL);
		}
		return target;
	}

	public static StringBuilder Ref_Merge(StringBuilder target, params string[] lines) {
		if(lines == null)
			return target;
		int c = lines.Length;
		if(c == 0)
			return target;
		for(int i = 0; i < c; i++)
		{
			target.Append(lines[i]).Append(NL);
		}
		return target;
	}

	public static StringBuilder Merge(StringBuilder[] lines, bool reutilizeFirst = false) {
		if(lines == null || lines.Length == 0)
			return null;
		int count = lines.Length;
		if(reutilizeFirst)
		{
			if(count > 1)
				return Ref_Merge(lines[0], lines.Slice(1, count - 1));
			else
				return lines[0];
		} else
			return Ref_Merge(new StringBuilder(), lines);
	}

	public static StringBuilder Merge(params StringBuilder[] lines) {
		if(lines == null || lines.Length == 0)
			return null;
		return Merge(lines, false);
	}

	public static StringBuilder Merge(List<StringBuilder> lines, bool reutilizeFirst = false) =>
		Merge(lines.ToArray(), reutilizeFirst);

	public static StringBuilder Merge(params string[] lines) {
		if(lines == null || lines.Length == 0)
			return null;

		return Ref_Merge(new StringBuilder(), lines);
	}

	public static StringBuilder Merge(List<string> lines) =>
		Merge(lines.ToArray());

	#endregion

	public static StringBuilder Ref_AddVariables(StringBuilder target, string type,
		List<(string, bool)> items) {
		if(items == null || items.Count == 0)
			return null;
		string[] converted = items.Select(x => (x.Item2 ? "public " : "private ") + type + ' ' + x.Item1 + ';').ToArray();
		return Ref_Merge(target, converted);

	}
}

public class ScriptComponentPackage {
	public List<string> usings = new List<string> {
		"System","System.Collections.Generic","UnityEngine"
	};

	public string className;

	public List<(string, bool)> ints;
	public List<(string, bool)> floats;
	public List<(string, bool)> strings;
	public List<(string, bool)> bools;

	public StringBuilder startMethod;
	public StringBuilder updateMethod;
	public List<(string name, StringBuilder body, bool isPublic, string type)> additionalMethods;

	public string Assemble() {
		var fullScript = new StringBuilder();
		_ = ScriptAssembler.Ref_Merge(fullScript, usings.Select(x => $"using {x};").ToArray())?.Append(ScriptAssembler.NL);

		var classBody = new StringBuilder();
		_ = ScriptAssembler.Ref_AddVariables(classBody, "int", ints)?.Append(ScriptAssembler.NL);
		_ = ScriptAssembler.Ref_AddVariables(classBody, "float", floats)?.Append(ScriptAssembler.NL);
		_ = ScriptAssembler.Ref_AddVariables(classBody, "string", strings)?.Append(ScriptAssembler.NL);
		_ = ScriptAssembler.Ref_AddVariables(classBody, "bool", bools)?.Append(ScriptAssembler.NL);

		_ = ScriptAssembler.Ref_FormMethod(classBody, "Start", startMethod, false)?.Append(ScriptAssembler.NL);
		_ = ScriptAssembler.Ref_FormMethod(classBody, "Update", updateMethod, false)?.Append(ScriptAssembler.NL);
		if(additionalMethods != null)
			foreach(var item in additionalMethods)
			{
				_ = ScriptAssembler.Ref_FormMethod(classBody,
					item.name, item.body, item.isPublic, item.type)?.Append(ScriptAssembler.NL);
			}


		ScriptAssembler.Ref_Enclose(
			fullScript, $"public class {className} : MonoBehaviour", classBody);
		return fullScript.ToString();
	}
}
