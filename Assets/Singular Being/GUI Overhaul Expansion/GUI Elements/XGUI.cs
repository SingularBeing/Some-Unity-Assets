using UnityEngine;
using System.Collections;

//using UnityEditor;

//using System;
//using System.IO;
//using System.Xml;
using System.Collections.Generic;

/*
	[Script Info]

*/
public class XGUI
{
	/// <summary>
	/// Title the specified rect and content.
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="content">Content.</param>
	public static void Title (Rect rect, string content)
	{
		GUISkin skin = (GUISkin)Resources.Load ("Skins/XGUI");
		GUIStyle style = new GUIStyle (ReturnStyle (skin, "Title"));
		GUI.Label (rect, content, style);
	}

	/// <summary>
	/// Title the specified rect, content and skin.
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="content">Content.</param>
	/// <param name="skin">Skin.</param>
	public static void Title (Rect rect, string content, GUISkin skin)
	{
		GUISkin _skin = skin;
		GUIStyle style = new GUIStyle (ReturnStyle (skin, "Title"));
		GUI.Label (rect, content, style);
	}

	/// <summary>
	/// Title the specified rect, content and style.
	/// </summary>
	/// <param name="rect">Rect.</param>
	/// <param name="content">Content.</param>
	/// <param name="style">Style.</param>
	public static void Title (Rect rect, string content, GUIStyle style)
	{
		GUIStyle _style = new GUIStyle (style);
		GUI.Label (rect, content, _style);
	}

	/// <summary>
	/// A Box with no Content.
	/// </summary>
	/// <param name="rect">Rect.</param>
	public static void BoxNoContent (Rect rect)
	{
		GUI.Box (rect, GUIContent.none);
	}

	public static void MenuBar (Rect rect, string[] titles, ref int selected)
	{
		//draw each button
		for (int i = 0; i < titles.Length; i++) {
			if (GUI.Button (new Rect (rect.x + (80f * i), 0, 80f, 20f), titles [i])) {
				selected = i;
			}
		}
	}

	static GUIStyle ReturnStyle (GUISkin skin, string value)
	{
		GUIStyle[] styles = skin.customStyles;
		foreach (GUIStyle style in styles) {
			if (style.name == value)
				return style;
		}

		return null;
	}

}