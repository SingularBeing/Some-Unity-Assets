using UnityEngine;
using System.Collections;
using CYRO;
using UnityEditor;

namespace CYRO
{

	public class HierarchyBackground
	{

		//this displays the hierarchy background in-game
		public HierarchyBackground (Rect rect, Color backgroundColor)
		{
			EditorGUI.DrawRect (rect, backgroundColor);
		}

	}

}