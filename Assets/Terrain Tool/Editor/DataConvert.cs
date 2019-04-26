using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using CustomMSLibrary.Standalone;
using CustomMSLibrary.Unity;

public class DataConvert {
	public string fullDocument;

	#region "Constants"
	private string O_BRACE = "{\r\n";
	private string C_BRACE = "\r\n}";

	public enum NewLineFormats { Windows_CR_LF, Unix_LF, Mac_CR }
	private NewLineFormats newLineFormat;
	public NewLineFormats NewLineFormat {
		get => newLineFormat;
		set {
			newLineFormat = value;
			switch(value)
			{
				case NewLineFormats.Windows_CR_LF:
					NL = "\r\n";
					break;
				case NewLineFormats.Unix_LF:
					NL = "\n";
					break;
				case NewLineFormats.Mac_CR:
					NL = "\r";
					break;
				default:
					break;
			}
		}
	}
	public string NL { get; private set; }

	private const string EMPTY = "";
	#endregion

	public string usings;
	public string className;

	public string startMethod;
	public string updateMethod;
	public List<string> additionalMethods;

	public DataConvert(NewLineFormats newLineFormat = NewLineFormats.Windows_CR_LF) {
		NewLineFormat = newLineFormat;
		O_BRACE = '{' + NL;
		C_BRACE = NL + '}';
	}

	#region Enclose
	public StringBuilder Enclose(string precedingText, string innerContent) {
		return new StringBuilder(precedingText)
			.Append(O_BRACE)
			.Append(innerContent)
			.Append(C_BRACE)
			.Append(NL);
	}

	public StringBuilder Enclose(StringBuilder precedingText, StringBuilder innerContent) =>
		Enclose(precedingText.ToString(), innerContent.ToString());

	public StringBuilder Enclose(StringBuilder precedingText, string innerContent) =>
		Enclose(precedingText.ToString(), innerContent);

	public StringBuilder Enclose(string precedingText, StringBuilder innerContent) =>
		Enclose(precedingText, innerContent.ToString()); 
	#endregion

	public StringBuilder InsertIf(string condition, string trueContent, string elseContent = EMPTY) {
		var newString = Enclose(
			new StringBuilder("if(").Append(condition).Append(')'),
			trueContent);
		if(elseContent != null && elseContent != EMPTY)
			newString = Enclose(newString.Append("else"), elseContent);
		return newString;
	}

	#region InsertFor
	public StringBuilder InsertFor(string counter, string count, string body) =>
	Enclose($"for(int {counter} = 0; {counter} < {count}; {counter}++)",
		body);

	public StringBuilder InsertFor(string counter, int count, string body) =>
		InsertFor(counter, count.ToString(), body); 
	#endregion



}
