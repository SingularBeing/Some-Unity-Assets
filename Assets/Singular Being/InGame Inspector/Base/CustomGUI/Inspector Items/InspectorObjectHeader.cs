using UnityEngine;
using System.Collections;
using CYRO;
using UnityEditor;

namespace CYRO
{

	public class InspectorObjectHeader
	{

		public static bool _enabled = true;

		public static void Init (Rect rect, GameObject obj, Color topBarColor, InGameGUI gui)
		{
			bool showToggle = false;

			string objName = obj.name;
			string objTag = obj.tag;
			//string objLayer = obj.layer;
			bool objStatic = obj.isStatic;
			bool objEnabled = obj.activeInHierarchy;

			_enabled = objEnabled;

			showToggle = true;

			//draw the top background
			EditorGUI.DrawRect (rect, topBarColor);

			//draw the enabled toggle first
			if (showToggle) {
				_enabled = GUI.Toggle (new Rect (5f, 5f, 20f, 20f), _enabled, "", EditorStyles.toggle);
				if (obj.activeInHierarchy != _enabled) {
					obj.SetActive (_enabled);
				}
			}
			//draw the name
			/*EditorGUI.BeginChangeCheck ();*/
			GUI.Label (new Rect (25f, 5f, rect.width - 100f, 20f), objName, EditorStyles.boldLabel);
			/*if (EditorGUI.EndChangeCheck ()) {
				Debug.Log ("ene");
			}*/
		}

	}

}