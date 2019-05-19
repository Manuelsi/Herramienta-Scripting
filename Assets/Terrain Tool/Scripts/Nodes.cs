using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class DrawnNode
{
    public string nodeName;
	public DrawnNode nextNode;

	public abstract string Content { get; }
	public abstract string NodeType { get; }

	private Rect _myRect;
    public Rect MyRect {
		get => _myRect;
		private set => _myRect = value;
	}

	public Vector2 RectPos {
		get => MyRect.position;
		set {
			_myRect.position = value;
		}
	}

	public virtual Vector2 ArrowTargetPos {
		get => new Vector2(RectPos.x, MyRect.y + (MyRect.height / 2f));
	}

	public virtual Vector2 ArrowSourcePos {
		get => new Vector2(RectPos.x + MyRect.width, MyRect.y + (MyRect.height / 2f));
	}

    public DrawnNode(Rect rect)
    {
		MyRect = rect;
    }

    public bool CheckMouse(Event cE, Vector2 pan)=>
        MyRect.Contains(cE.mousePosition - pan);
    
	public virtual void DrawLine(DrawnNode target) =>
		Handles.DrawLine(ArrowSourcePos, target.ArrowTargetPos);

    //public bool OverNode
    //{ get { return _overNode; } }


}
