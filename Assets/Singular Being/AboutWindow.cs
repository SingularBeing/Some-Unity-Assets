using UnityEngine;
using System.Collections;
using UnityEditor;

//using System;
using System.IO;

//using System.Xml;
using System.Collections.Generic;

/*
	[Script Info]

*/
public class AboutWindow : EditorWindow
{
	string[] names = new string[] {
		"Andrew Burke"	
	};

	static List<string> packages = new List<string> ();

	//[MenuItem ("Singular Being/About")]
	static void Open ()
	{
		AboutWindow window = (AboutWindow)EditorWindow.GetWindowWithRect (typeof(AboutWindow), new Rect (100f, 100f, 570f, 340f), true, "About Singular Being");

		string[] direcs = Directory.GetDirectories (Application.dataPath + "/Singular Being/");
		foreach (string s in direcs) {
			packages.Add (s.Split ('/') [s.Split ('/').Length - 1]);
		}
	}

	void OnGUI ()
	{
		//icon TBA
		//display team members
		Rect nameRect = new Rect (this.position.size.x - 120, 0, 120, this.position.size.y);
		GUI.Box (nameRect, GUIContent.none);
		GUILayout.BeginArea (nameRect);
		{
			GUILayout.BeginVertical ();
			{
				//display the programmers
				GUILayout.Label ("Meet the Team");
				GUILayout.Space (25);
				GUILayout.Label ("Programmers:");
				GUILayout.Label ("Andrew Burke");

				EditorGUI.indentLevel--;
				//display the artists

			}
			GUILayout.EndVertical ();
		}
		GUILayout.EndArea ();
		Rect packagesRect = new Rect (0, this.position.size.y / 2, this.position.size.x - 200, this.position.size.y / 2);
		//display all packages bought by singular being
		GUILayout.BeginArea (packagesRect);
		{
			GUILayout.BeginVertical ();
			{
				GUILayout.Label ("Packages Bought:");
				GUILayout.Space (10);
				foreach (string s in packages) {
					GUILayout.Label (s);
				}
			}
			GUILayout.EndVertical ();
		}
		GUILayout.EndArea ();
	}

}