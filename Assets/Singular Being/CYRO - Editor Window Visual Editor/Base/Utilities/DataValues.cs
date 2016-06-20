using UnityEngine;
using System.Collections;

namespace CYRO.EditorWindowVisualEditor
{

	/// <summary>
	/// Contains all of the base GUI, GUILayout, EditorGUI, and EditorGUI properties
	/// </summary>
	public static class DataValues
	{

		/// <summary>
		/// GUI.Label properties
		/// -<rect> The rect </rect>
		/// -<content> The content (text, image)</content>
		/// --<content=text> The text </content>
		/// --<content=image> The image </content>
		/// -<style> The style </style>
		/// </summary>
		public const string gui_label = "<rect> </rect> <content> <content=text> <content=image> </content> <style> </style>";

	}

}