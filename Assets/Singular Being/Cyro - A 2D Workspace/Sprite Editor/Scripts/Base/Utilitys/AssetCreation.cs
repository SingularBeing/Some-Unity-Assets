using UnityEngine;
using System.Collections;
using CYRO;
using UnityEditor;
using System.IO;

namespace CYRO
{

	public static class AssetCreation
	{

		private static void CheckForDirectories ()
		{
			if (!Directory.Exists (Application.dataPath + "/Data/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/");
			if (!Directory.Exists (Application.dataPath + "/Data/Sprites/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/Sprites/");
			if (!Directory.Exists (Application.dataPath + "/Data/Rooms/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/Rooms/");
			if (!Directory.Exists (Application.dataPath + "/Data/Objects/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/Objects/");
			if (!Directory.Exists (Application.dataPath + "/Data/Backgrounds/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/Backgrounds/");
			if (!Directory.Exists (Application.dataPath + "/Data/Paths/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/Paths/");
			if (!Directory.Exists (Application.dataPath + "/Data/Timelines/"))
				Directory.CreateDirectory (Application.dataPath + "/Data/Timelines/");

			AssetDatabase.Refresh ();
		}

		public static UnityEngine.Object objectType;

		public static void Create_Sprite ()
		{
			CheckForDirectories ();

			SSprite sprite = (SSprite)ScriptableObject.CreateInstance (typeof(SSprite));
			objectType = sprite;
			AssetDatabase.CreateAsset (objectType, FileUtil.GetProjectRelativePath (Application.dataPath + "/Data/Sprites/spr_new" + Random.Range (0, int.MaxValue) + ".asset"));
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();

			E_MainWindow.updateAllAssets = true;
		}

		public static void Create_Room ()
		{
			CheckForDirectories ();

			SRoom sprite = (SRoom)ScriptableObject.CreateInstance (typeof(SRoom));
			objectType = sprite;
			AssetDatabase.CreateAsset (objectType, FileUtil.GetProjectRelativePath (Application.dataPath + "/Data/Rooms/rm_new" + Random.Range (0, int.MaxValue) + ".asset"));
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();

			E_MainWindow.updateAllAssets = true;
		}

		public static void Create_Object ()
		{
			CheckForDirectories ();

			SObject sprite = (SObject)ScriptableObject.CreateInstance (typeof(SObject));
			objectType = sprite;
			AssetDatabase.CreateAsset (objectType, FileUtil.GetProjectRelativePath (Application.dataPath + "/Data/Objects/obj_new" + Random.Range (0, int.MaxValue) + ".asset"));
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();

			E_MainWindow.updateAllAssets = true;
		}

		public static void Create_Background ()
		{
			CheckForDirectories ();

			SBackground sprite = (SBackground)ScriptableObject.CreateInstance (typeof(SBackground));
			objectType = sprite;
			AssetDatabase.CreateAsset (objectType, FileUtil.GetProjectRelativePath (Application.dataPath + "/Data/Backgrounds/bg_new" + Random.Range (0, int.MaxValue) + ".asset"));
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();

			E_MainWindow.updateAllAssets = true;
		}

		public static void Create_Path ()
		{
			CheckForDirectories ();

		}

		public static void Create_Timeline ()
		{
			CheckForDirectories ();

		}

	}

}