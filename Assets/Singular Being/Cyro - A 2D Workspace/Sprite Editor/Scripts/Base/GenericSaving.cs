using UnityEngine;
using System.Collections;
using System.IO;
using CYRO;

namespace CYRO
{

	public static class GenericSaving
	{
		static readonly string saveFilePath = Application.persistentDataPath + "/Singular Being/saveFile.txt";

		public static void SetString (string key, string value)
		{

			if (!Directory.Exists (Application.persistentDataPath + "/Singular Being"))
				Directory.CreateDirectory (Application.persistentDataPath + "/Singular Being");

			CheckForFile ();

			//set a string in a txt file
			StreamReader read = new StreamReader (saveFilePath);
			string[] lines = read.ReadToEnd ().Split ('\n');
			read.Close ();

			string finalString = "";

			int index = -1;

			for (int i = 0; i < lines.Length; i++) {
				if (lines [i].Contains ("[" + key + "]")) {
					finalString += "[" + key + "]" + value + '\n';
				} else {
					finalString += lines [i] + '\n';
				}
			}

			if (!finalString.Contains ("[" + key + "]"))
				finalString += "[" + key + "]" + value + '\n';

			StreamWriter write = new StreamWriter (saveFilePath);
			write.Write (finalString);
			write.Close ();
		}

		public static string GetString (string key)
		{
			if (!Directory.Exists (Application.persistentDataPath + "/Singular Being"))
				Directory.CreateDirectory (Application.persistentDataPath + "/Singular Being");

			CheckForFile ();

			StreamReader read = new StreamReader (saveFilePath);
			string[] lines = read.ReadToEnd ().Split ('\n');
			read.Close ();

			foreach (string s in lines) {
				if (s.Contains ("[" + key + "]")) {
					return s.Split (']') [1];
				}
			}

			return null;
		}

		public static bool GetBool (string key)
		{
			if (!Directory.Exists (Application.persistentDataPath + "/Singular Being"))
				Directory.CreateDirectory (Application.persistentDataPath + "/Singular Being");

			CheckForFile ();

			StreamReader read = new StreamReader (saveFilePath);
			string[] lines = read.ReadToEnd ().Split ('\n');
			read.Close ();

			foreach (string s in lines) {
				if (s.Contains ("[" + key + "]")) {
					return s.Split (']') [1] == "true" ? true : false;
				}
			}

			return false;
		}

		static void CheckForFile ()
		{
			if (!File.Exists (saveFilePath)) {
				StreamWriter write = new StreamWriter (saveFilePath);
				write.Write ("");
				write.Close ();
			}
		}

	}

}