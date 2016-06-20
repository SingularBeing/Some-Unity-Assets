using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace CYRO.EditorWindowVisualEditor
{

	public static class Utility
	{

		/// <summary>
		/// The default location for the .txt files.
		/// </summary>
		const string _defaultTxtLocation = "";

		public static string[] LoadEditorWindowFromFile (string location)
		{
			StreamReader read = new StreamReader (location);
			string[] content = read.ReadToEnd ().Split ('\n');
			read.Close ();

			return content;
		}

		public static string[] LoadEditorWindowFromFile ()
		{
			StreamReader read = new StreamReader (Application.dataPath + "\\sample.txt");
			string[] content = read.ReadToEnd ().Split ('\n');
			read.Close ();

			return content;
		}

		/// <summary>
		/// Splits the text file to get the tag data
		/// </summary>
		/// <param name="guiType">What GUI element are we using?</param>
		/// <param name="lines">Split lines from LoadEditorWindowFromFile(string location).</param>
		public static List<GUIElement_Base> SplitTxtFile (string[] lines)
		{
			//set up the list of GUIElements
			List<GUIElement_Base> displayElements = new List<GUIElement_Base> ();
			//check the lines for these values
			for (int i = 0; i < lines.Length; i++) {
				GUIElement_Display element = new GUIElement_Display ();
				string guiType = lines [i].Substring (0, lines [i].IndexOf ('>'));
				//Debug.Log (guiType);
				//split this line into smaller pieces, components
				string[] splitForComponents = SplitComponentsNotValues (lines [i]);
				for (int i2 = 0; i2 < splitForComponents.Length; i2++) {
					if (guiType == "GUI.Label") {
						if (splitForComponents [i2].Contains ("<rect>")) {
							//grab the index of the closing tag
							int indexOfClosing = splitForComponents [i2].IndexOf ("</rect>") - 1;
							//grab the rect
							string cutOffFirst = splitForComponents [i2].Substring (6, splitForComponents [i2].Substring (6).Length - 7);
							//Debug.Log (cutOffFirst);
							//split into 4 pieces, X, Y, W, H
							//Debug.Log (cutOffFirst);
							string[] splitRectValues = cutOffFirst.Split (' ');
							element.myRect = new Rect (float.Parse (splitRectValues [0]), float.Parse (splitRectValues [1]), float.Parse (splitRectValues [2]), float.Parse (splitRectValues [3]));
							//Debug.Log (element.myRect);
						}
						if (splitForComponents [i2].Contains ("<content=")) {
							//grab the index of the closing tag
							int indexOfClosing = splitForComponents [i2].IndexOf ("</content>") - 1;
							int indexOfFirstClosing = splitForComponents [i2].IndexOf ('>') - 1;
							//grab the content type
							string cutOffForType = splitForComponents [i2].Substring (9, indexOfFirstClosing - 8);
							//grab the text
							string text = splitForComponents [i2].Substring (indexOfFirstClosing + 2, splitForComponents [i2].Length - indexOfFirstClosing - "</content>".Length - 2);
							//Debug.Log (text);
							element.myContent = new GUIContent (text);
							if (cutOffForType == "image")
								element.useTexture = true;
						}
						if (splitForComponents [i2].Contains ("<style>")) {
							//grab the index of the closing tag
							string cutOffForStyle = splitForComponents [i2].Substring (7, splitForComponents [i2].Length - "</style>".Length - 7);
							//Debug.Log (cutOffForStyle);
						}
					}
				}
				element.myIndentifier = guiType;
				element.myName = "Label (" + Random.Range (0, int.MaxValue) + ")";
				//Debug.Log (element.myIndentifier);
				displayElements.Add (element);
			}

			return displayElements;
		}

		static string[] SplitComponentsNotValues (string line)
		{
			string replacedLine = line.Replace ("> ", ">|");
			string[] splitLine = replacedLine.Split ('|');
			return splitLine;
		}

		static string GetGUIDataValue (string guiType)
		{
			string currentDataValue = "";
			if (guiType == "GUI.Label")
				return DataValues.gui_label;

			return currentDataValue;
		}

		public static GUIStyle GetGUIStyle (string nameOf)
		{
			foreach (GUIStyle style in GUI.skin.customStyles) {
				if (style.name == nameOf)
					return style;
			}

			return null;
		}

		public static GUIStyle GetGUIStyleFromResources (string nameOf)
		{
			GUISkin skin = (GUISkin)Resources.Load ("Skins/VisualEditorSkin");
			foreach (GUIStyle style in skin.customStyles) {
				if (style.name == nameOf)
					return style;
			}

			return null;
		}
	}

}