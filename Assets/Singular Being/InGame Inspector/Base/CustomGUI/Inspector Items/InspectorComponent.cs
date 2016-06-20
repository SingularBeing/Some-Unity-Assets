using UnityEngine;
using System.Collections;
using CYRO;
using System.Reflection;
using System;
using UnityEditor;

namespace CYRO
{

	public class InspectorComponent
	{
		static bool _enabled;

		public static void Init (Rect rect, MonoBehaviour component, Color backColor)
		{
			Type componentType = component.GetType ();
			EditorGUI.DrawRect (rect, backColor);

			bool componentEnabled = component.enabled;

			_enabled = componentEnabled;

			//toggle
			_enabled = EditorGUI.ToggleLeft (new Rect (rect.x + 10, rect.y + 10, 20, 20), "", _enabled);
			if (component.enabled != _enabled) {
				component.enabled = _enabled;
			}
			//name
			GUI.Label (new Rect (rect.x + 10 + 30, rect.y + 10, 200f, 20f), componentType.Name);
		}

	}

}