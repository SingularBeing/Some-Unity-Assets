using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using CYRO;

namespace CYRO
{
	/*
	[Script Info]

*/


	public class ProjectEditorWindow : EditorWindow
	{

		public static ProjectEditorWindow window;

		public List<SSprite> sprites = new List<SSprite> ();
		public List<SRoom> rooms = new List<SRoom> ();
		public List<SBackground> backgrounds = new List<SBackground> ();

		string typeText = "";
		bool wholeWordOnly;
		bool filterTree;

		[MenuItem ("CYRO/Windows/Project Hierarchy")]
		public static void Open ()
		{
			window = (ProjectEditorWindow)GetWindow (typeof(ProjectEditorWindow), false, "Project");
		}

		static bool spriteFoldout = true;
		static bool objectFoldout = true;
		static bool timelineFoldout = true;
		static bool roomFoldout = true;
		static bool backgroundFoldout = true;

		void OnProjectStart ()
		{
			sprites.Clear ();
			rooms.Clear ();
			backgrounds.Clear ();

			string[] allAssets = AssetDatabase.GetAllAssetPaths ();

			foreach (string asset in allAssets) {
				if (asset.EndsWith (".asset")) {
					//check for type
					if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("spr_")) {
						//these are the sprites
						sprites.Add ((SSprite)AssetDatabase.LoadAssetAtPath (asset, typeof(SSprite)) as SSprite);
						//sprites [sprites.Count - 1].assetLocation = asset;
					} else if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("rm_")) {
						rooms.Add ((SRoom)AssetDatabase.LoadAssetAtPath (asset, typeof(SRoom)) as SRoom);
					} else if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("bg_")) {
						backgrounds.Add ((SBackground)AssetDatabase.LoadAssetAtPath (asset, typeof(SBackground)) as SBackground);
					}
				}
			}
		}

		void OnFocus ()
		{
			OnProjectStart ();
			window = this;
		}

		void OnGUI ()
		{
			//all the sprites
			spriteFoldout = EditorGUILayout.Foldout (spriteFoldout, "Sprites");
			if (spriteFoldout) {
				foreach (SSprite sprite in sprites) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label (sprite.name);
						if (GUILayout.Button ("Edit", GUILayout.Width (75f))) {
							/*	MainSpriteEditor window = (MainSpriteEditor)GetWindow (typeof(MainSpriteEditor), true, "Sprite Editor");
						MainSpriteEditor.currentObject = sprite;
						window.images = MainSpriteEditor.currentObject.textures;*/
							//SpriteEditorEditor.Open (sprite);
							E_SpriteEditorWindow.Open (sprite);
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
			objectFoldout = EditorGUILayout.Foldout (objectFoldout, "Objects");
			if (objectFoldout) {
			
			}
			roomFoldout = EditorGUILayout.Foldout (roomFoldout, "Rooms");
			if (roomFoldout) {
				foreach (SRoom room in rooms) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label (room.name);
						if (GUILayout.Button ("Edit", GUILayout.Width (75f))) {
							//RoomsEditor.Open ();
							//RoomsEditor.current = room;
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
			backgroundFoldout = EditorGUILayout.Foldout (backgroundFoldout, "Backgrounds");
			if (backgroundFoldout) {
				foreach (SBackground background in backgrounds) {
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Label (background.name);
						if (GUILayout.Button ("Edit", GUILayout.Width (75f))) {
							//BackgroundsEditor.current = background;
							//BackgroundsEditor.Open ();
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
			timelineFoldout = EditorGUILayout.Foldout (timelineFoldout, "Timelines");
			if (timelineFoldout) {

			}
			//make a text field
			GUI.Box (new Rect (5f, window.position.size.y - 90f, window.position.size.x - 10f, 80f), "");
			typeText = GUI.TextField (new Rect (15f, window.position.size.y - 80f, window.position.size.x - 30f, 20f), typeText);
			wholeWordOnly = EditorGUI.ToggleLeft (new Rect (15f, window.position.size.y - 60f, window.position.size.x / 2 - 15f, 20f), "Whole Word Only", wholeWordOnly);
			filterTree = EditorGUI.ToggleLeft (new Rect (window.position.size.x / 2, window.position.size.y - 60f, window.position.size.x / 2 - 15f, 20f), "Filter Tree", filterTree);
			if (GUI.Button (new Rect (15f, window.position.size.y - 40f, window.position.size.x / 2 - 15f, 20f), "Previous")) {
			
			}
			if (GUI.Button (new Rect (window.position.size.x / 2, window.position.size.y - 40f, window.position.size.x / 2 - 15f, 20f), "Next")) {

			}
		}

	}

}