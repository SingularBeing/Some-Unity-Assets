using UnityEngine;
using System.Collections;
using UnityEditor;

//using System;
//using System.IO;
//using System.Xml;
//using System.Collections.Generic;

/*
	[Script Info]

*/
public class ApexMenu : EditorWindow
{

	[MenuItem ("Window/Apex Pathfinding/About")]
	static void About ()
	{
		
	}

	[MenuItem ("Window/Apex Pathfinding/Create/Smooth Path")]
	static void CreateSmoothPath ()
	{
		GameObject obj = new GameObject ("Smooth_Path");
		obj.AddComponent<BezierSpline> ();
		Selection.activeGameObject = obj;
	}



}