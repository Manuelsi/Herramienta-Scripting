using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Linq;
using UnityEditor;

public class Placeholder : MonoBehaviour
{
	public string className;
	public string ClassPath => className + ".cs";
	public string ClassFullPath => Application.dataPath + Path.DirectorySeparatorChar + ClassPath;

	[TextArea]
	public string start;

	[TextArea]
	public string update;

	[TextArea]
	public string blahMethod;

	public List<string> ints;
	public List<string> strings;



	// Start is called before the first frame update
	void Start() {
		var data = new ScriptComponentPackage() {
			additionalMethods = new List<(string, StringBuilder, bool, string)> { ("Blah", new StringBuilder(blahMethod), false, "void") },
			className = "Testero",
			ints = ints.Select(x => (x, true)).ToList(),
			strings = strings.Select(x => (x, true)).ToList(),
			startMethod = new StringBuilder(start),
			updateMethod = new StringBuilder(update),
		};
		string textData = data.Assemble();
		if(!File.Exists(ClassFullPath))
			File.CreateText(ClassFullPath).Dispose();
		File.WriteAllText(ClassFullPath, textData);
	}

}
