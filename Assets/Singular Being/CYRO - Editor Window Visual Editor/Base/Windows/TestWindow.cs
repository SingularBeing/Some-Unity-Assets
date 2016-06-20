using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using CYRO.EditorWindowVisualEditor;

public class TestWindow : EditorWindow
{

	public static List<GUIElement_Base> elements = new List<GUIElement_Base> ();

	[MenuItem ("Sample/Sample")]
	public static void Open ()
	{
		elements = Utility.SplitTxtFile (Utility.LoadEditorWindowFromFile ());
		GetWindow (typeof(TestWindow), false, "Thing");
	}

	void OnGUI ()
	{
		GUILayout.Label ("Count:" + elements.Count);
		if (GUILayout.Button ("Refresh")) {
			elements.Clear ();
			elements = Utility.SplitTxtFile (Utility.LoadEditorWindowFromFile ());
		}

		foreach (GUIElement_Base b in elements) {
			if (b.myIndentifier == "GUI.Label") {
				GUI.Label (b.myRect, b.myContent);
			}
		}
	}

}