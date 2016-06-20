using UnityEngine;
using System.Collections;
using CYRO;
using UnityEditor;

namespace CYRO
{

	public class HierarchyGameObject
	{

		//this displays the gameObject assigned to this element
		public static bool Init (Rect rect, GameObject sceneObj, int indentLevel)
		{
			string _objectName = sceneObj.name;
			//draw the background
			//EditorGUI.DrawRect (rect, backgroundColor);
			//draw the item
			if (GUI.Button (new Rect (rect.x + (indentLevel * 20), rect.y, rect.width, rect.height), _objectName, EditorStyles.toolbarButton))
				return true;
			else
				return false;
		}

	}

}