using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class WindowsScripts : EditorWindow {
	private List<DrawnNode> allNodes;
	private GUIStyle myStyle;
	private float toolbarHeight = 100;
	private string currentName;

    
	private DrawnNode _selectedNode;
    private DrawnNode nxNode;
	private bool _panningScreen;
	private Vector2 graphPan;
	private Rect graphRect;
	private Vector2 _originalMousePosition;
	private Vector2 prevPan;
	public GUIStyle wrapTextFieldStyle;
	[MenuItem("CustomTools/ Easy Script")]
	public static void OpenWindows() {
		var mySelf = GetWindow<WindowsScripts>();
		mySelf.allNodes = new List<DrawnNode>();
		mySelf.myStyle = new GUIStyle();
		mySelf.myStyle.fontSize = 20;
		mySelf.myStyle.alignment = TextAnchor.MiddleCenter;
		mySelf.myStyle.fontStyle = FontStyle.BoldAndItalic;

		mySelf.graphPan = new Vector2(0, mySelf.toolbarHeight);
		mySelf.graphRect = new Rect(0, mySelf.toolbarHeight, 1000000, 1000000);


		mySelf.wrapTextFieldStyle = new GUIStyle(EditorStyles.textField);
		mySelf.wrapTextFieldStyle.wordWrap = true;
		mySelf.wantsMouseMove = true;
	}
	private void OnGUI() {
		CheckMouseInput(Event.current);

		EditorGUILayout.BeginVertical(GUILayout.Height(100));
		EditorGUILayout.LabelField("Script Easy", myStyle, GUILayout.Height(50));
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
        
        currentName = EditorGUILayout.TextField("Nombre: ", currentName);
		EditorGUILayout.Space();
		if(GUILayout.Button("Create Script", GUILayout.Width(100), GUILayout.Height(30)))
			AddNode();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();


		graphRect.x = graphPan.x;
		graphRect.y = graphPan.y;
		EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.gray);


		GUI.BeginGroup(graphRect);

		BeginWindows();
		var oriCol = GUI.backgroundColor;
		for(int i = 0; i < allNodes.Count; i++)
		{
			allNodes[i].DrawConnections();
			/*Handles.DrawLine(
				new Vector2(allNodes[i].MyRect.position.x + allNodes[i].MyRect.width / 2f,
					allNodes[i].MyRect.position.y + allNodes[i].MyRect.height / 2f),

				new Vector2(allNodes[i].next.MyRect.position.x + allNodes[i].next.MyRect.width / 2f,
					allNodes[i].next.MyRect.position.y + allNodes[i].next.MyRect.height / 2f));
			*/
		}

		for(int i = 0; i < allNodes.Count; i++)
		{
			if(allNodes[i] == _selectedNode)
				GUI.backgroundColor = Color.gray;

			var nodeRect = GUI.Window(i, allNodes[i].MyRect, DrawNode, allNodes[i].NodeType); //TODO: crear metodo dentro DrawnNode
			allNodes[i].RectPos = nodeRect.position;

			GUI.backgroundColor = oriCol;
		}
		EndWindows();
		GUI.EndGroup();
	}
	private void CheckMouseInput(Event currentE) {

        if (currentE.button == 0&& currentE.type==EventType.MouseDown)
        {
            GenericMenu genericMenu = new GenericMenu();

			foreach(var item in allNodes)
			{
				if(item.MyRect.Contains(currentE.mousePosition))
				{
					nxNode = item;
					break;
				}
			}
			genericMenu.AddItem(new GUIContent("Unir Nodos"), false, JoinNodes);
        }

		if(!graphRect.Contains(currentE.mousePosition) || !(focusedWindow == this || mouseOverWindow == this))
			return;


		if(currentE.button == 2 && currentE.type == EventType.MouseDown)
		{
			_panningScreen = true;
			prevPan = new Vector2(graphPan.x, graphPan.y);
			_originalMousePosition = currentE.mousePosition;
		} else if(currentE.button == 2 && currentE.type == EventType.MouseUp)
			_panningScreen = false;

		if(_panningScreen)
		{
			var newX = prevPan.x + currentE.mousePosition.x - _originalMousePosition.x;
			graphPan.x = newX > 0 ? 0 : newX;

			var newY = prevPan.y + currentE.mousePosition.y - _originalMousePosition.y;
			graphPan.y = newY > toolbarHeight ? toolbarHeight : newY;

			Repaint();
		}

		DrawnNode overNode = null;
		for(int i = 0; i < allNodes.Count; i++)
		{

			if(allNodes[i].CheckMouse(Event.current, graphPan))
				overNode = allNodes[i];
		}

		var prevSel = _selectedNode;
		if(currentE.button == 0 && currentE.type == EventType.MouseDown)
		{
			if(overNode != null)
				_selectedNode = overNode;
			else
				_selectedNode = null;

			if(prevSel != _selectedNode)
				Repaint();
		}
	}

	private void JoinNodes() =>
		_selectedNode.SetNextNode(nxNode);

	private void AddNode() {
		allNodes.Add(new IfNode());
		currentName = "";
		Repaint();
	}
	private void DrawNode(int id) {



		if(!_panningScreen)
		{

			GUI.DragWindow();

			if(!allNodes[id].CheckMouse(Event.current, graphPan))
				return;

			float x = allNodes[id].RectPos.x < 0 ? 0 : allNodes[id].RectPos.x;
			float yOff = toolbarHeight - graphPan.y;
			float y = (allNodes[id].MyRect.y < yOff) ? yOff : allNodes[id].MyRect.y;

			allNodes[id].RectPos = new Vector2(x, y);
		}
	}
}
