using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Net;

public class ExampleWindow : EditorWindow
{

	[MenuItem ("Test/test")]
	public static void Init ()
	{
		ExampleWindow window = (ExampleWindow)GetWindow (typeof(ExampleWindow), true, "");
	}

	void OnGUI ()
	{
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		DrawToolStrip ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginArea (new Rect (0, 20f, 175, 400f));
		{
			GUILayout.BeginVertical ("BOX");
			GUILayout.Label ("Sprites", EditorStyles.boldLabel);
			GUILayout.EndVertical ();
		}
		GUILayout.EndArea ();

		Handles.BeginGUI ();
		Handles.DrawBezier (this.position.center, this.position.center, new Vector2 (this.position.xMax, this.position.center.y), new Vector2 (this.position.xMin - 50f, this.position.center.y - 50f), Color.red, null, 5f);
		Handles.EndGUI ();
	}

	void DrawToolStrip ()
	{
		if (GUILayout.Button ("Create...", EditorStyles.toolbarButton)) {
			OnMenu_Create ();
			EditorGUIUtility.ExitGUI ();
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button ("Tools", EditorStyles.toolbarDropDown)) {
			GenericMenu toolsMenu = new GenericMenu ();

			if (Selection.activeGameObject != null)
				toolsMenu.AddItem (new GUIContent ("Optimize Selected"), false, OnTools_OptimizeSelected);
			else
				toolsMenu.AddDisabledItem (new GUIContent ("Optimize Selected"));

			toolsMenu.AddSeparator ("");

			toolsMenu.AddItem (new GUIContent ("Help..."), false, OnTools_Help);

			toolsMenu.DropDown (new Rect (Screen.width - 216 - 40, 0, 0, 16));

			EditorGUIUtility.ExitGUI ();
		}
	}

	void OnMenu_Create ()
	{
		
	}

	void OnTools_OptimizeSelected ()
	{
		
	}

	void OnTools_Help ()
	{
		Help.BrowseURL ("");
	}

}