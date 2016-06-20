using UnityEngine;
using UnityEditor;
using System.Collections;
using CYRO;
using System.Collections.Generic;
using System;
using System.IO;

namespace CYRO
{

	public class E_MainWindow : EditorWindow
	{

		public static E_MainWindow window;

		public Vector2 windowSize {
			get {
				return window.position.size;
			}
		}

		public static List<SSprite> spriteAssets = new List<SSprite> ();
		public static List<SRoom> roomAssets = new List<SRoom> ();
		public static List<SBackground> backgroundAssets = new List<SBackground> ();
		public static List<SObject> objectAssets = new List<SObject> ();

		public static ScriptableObject currentObject;
		public static Type currentType;

		public static bool updateAllAssets = false;

		public Vector2 leftPanelScroll = Vector2.zero;
		public Vector2 rightPanelScroll = Vector2.zero;
		public Vector2 middlePanelScroll = Vector2.zero;

		public float currentZoomAmount = 1;

		public Rect middleView = new Rect ();
		public Rect middleImage = new Rect ();

		public bool update = false;

		public Material usableMaterial;
		public Material tempMaterial;
		public Texture2D tempTexture;
		public Texture2D refTexture;

		public bool canShowGUI = true;
		public bool showCollisions = false;

		public struct RightPanelSSprite
		{
			public bool showCollisionInfo;
		}

		[MenuItem ("CYRO/Windows/Main Window (Used for Docking)")]
		public static void Open ()
		{
			window = (E_MainWindow)GetWindow (typeof(E_MainWindow), false, "Main");
		}

		void OnFocus ()
		{
			if (currentObject != null) {
				canShowGUI = false;

				switch (currentType.Name) {
				case "SSprite":
					SSprite s = (SSprite)currentObject;
					if (!File.Exists (s.assetLocation)) {
						currentObject = null;
						currentType = null;
					}
					break;
				case "SObject":
					SObject o = (SObject)currentObject;
					if (!File.Exists (o.assetLocation)) {
						currentObject = null;
						currentType = null;
					}
					break;
				}

				canShowGUI = true;
			} else {
				canShowGUI = false;
				ImportAssets ();
				canShowGUI = true;
			}

			window = this;
		}

		void OnEnable ()
		{
			ImportAssets ();
		}

		void OnDisable ()
		{
			
		}

		void ImportAssets ()
		{
			spriteAssets.Clear ();
			roomAssets.Clear ();
			backgroundAssets.Clear ();
			objectAssets.Clear ();

			string[] allAssets = AssetDatabase.GetAllAssetPaths ();

			foreach (string asset in allAssets) {
				if (asset.EndsWith (".asset")) {
					//check for type
					if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("spr_")) {
						//these are the sprites
						spriteAssets.Add ((SSprite)AssetDatabase.LoadAssetAtPath (asset, typeof(SSprite)) as SSprite);
						spriteAssets [spriteAssets.Count - 1].assetLocation = asset;
					} else if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("rm_")) {
						roomAssets.Add ((SRoom)AssetDatabase.LoadAssetAtPath (asset, typeof(SRoom)) as SRoom);
					} else if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("bg_")) {
						backgroundAssets.Add ((SBackground)AssetDatabase.LoadAssetAtPath (asset, typeof(SBackground)) as SBackground);
					} else if (asset.Split ('/') [asset.Split ('/').Length - 1].StartsWith ("obj_")) {
						objectAssets.Add ((SObject)AssetDatabase.LoadAssetAtPath (asset, typeof(SObject)) as SObject);
					}
				}
			}
		}

		Rect leftPanel;
		Rect rightPanel;
		Rect middlePanel;

		Rect sobject_bottomPanel;

		void SetRects ()
		{
			leftPanel = new Rect (0, 20, 200f, windowSize.y - 20);
			rightPanel = new Rect (windowSize.x - 200f, 20f, 200f, windowSize.y - 20);
			middlePanel = new Rect (leftPanel.width, 20f, windowSize.x - leftPanel.width - rightPanel.width, windowSize.y - 20f);

			sobject_bottomPanel = new Rect (leftPanel.width, windowSize.y - 175f, windowSize.x - leftPanel.width - rightPanel.width, 175f);
		}

		void OnGUI ()
		{
			if (canShowGUI) {
				SetRects ();

				GUILayout.BeginHorizontal (EditorStyles.toolbar);
				{
					DrawToolStrip ();
				}
				GUILayout.EndHorizontal ();

				#region left-panel
				GUI.Box (leftPanel, "");
				//display the left most area for sprites, objects, etc
				GUILayout.BeginArea (leftPanel);
				{
					leftPanelScroll = GUILayout.BeginScrollView (leftPanelScroll);
					{
						//draw each section
						//sprites
						GUILayout.BeginVertical ();
						{
							GUILayout.Label ("Sprites", EditorStyles.boldLabel);
						}
						GUILayout.EndVertical ();
						GUILayout.BeginVertical ();
						{
							//draw the sprite names
							foreach (SSprite s in spriteAssets) {
								if (GUILayout.Button (s.name, EditorStyles.toolbarButton)) {
									OnSpriteLoad (s);
								}
							}
						}
						GUILayout.EndVertical ();

						//objects
						GUILayout.BeginVertical ();
						{
							GUILayout.Label ("Objects", EditorStyles.boldLabel);
						}
						GUILayout.EndVertical ();
						GUILayout.BeginVertical ();
						{
							//draw the object names
							foreach (SObject o in objectAssets) {
								if (GUILayout.Button (o.name, EditorStyles.toolbarButton)) {
									OnObjectLoad (o);
								}
							}
						}
						GUILayout.EndVertical ();

						//rooms
						GUILayout.BeginVertical ();
						{
							GUILayout.Label ("Rooms", EditorStyles.boldLabel);
						}
						GUILayout.EndVertical ();
						GUILayout.BeginVertical ();
						{
							//draw the room names
							foreach (SRoom r in roomAssets) {
								if (GUILayout.Button (r.name, EditorStyles.toolbarButton)) {
									currentType = typeof(SRoom);
									currentObject = r;
								}
							}
						}
						GUILayout.EndVertical ();

						//backgrounds
						GUILayout.BeginVertical ();
						{
							GUILayout.Label ("Backgrounds", EditorStyles.boldLabel);
						}
						GUILayout.EndVertical ();
						GUILayout.BeginVertical ();
						{
							//draw the room names
							foreach (SBackground b in backgroundAssets) {
								if (GUILayout.Button (b.name, EditorStyles.toolbarButton)) {
									currentType = typeof(SBackground);
									currentObject = b;
								}
							}
						}
						GUILayout.EndVertical ();

						//timelines
						GUILayout.BeginVertical ();
						{
							GUILayout.Label ("Timelines", EditorStyles.boldLabel);
						}
						GUILayout.EndVertical ();

						//paths
						GUILayout.BeginVertical ();
						{
							GUILayout.Label ("Paths", EditorStyles.boldLabel);
						}
						GUILayout.EndVertical ();
					}
					GUILayout.EndScrollView ();
				}
				GUILayout.EndArea ();
				#endregion
				#region middle-panel
				if (currentType != null && currentType.Name != "SObject") {
					GUI.BeginGroup (middlePanel);
					{
						if (currentObject != null) {
							//middle of this view
							switch (currentType.Name) {
							case "SSprite":
								SSprite s = (SSprite)currentObject;
						//if the texture is NOT null
								if (tempTexture != null) {
									//GUILayout.Label ("Panel Width:" + middlePanel.width + ", Image Width:" + middleImage.width);
									middlePanelScroll = GUI.BeginScrollView (new Rect (0, 0, middlePanel.width, middlePanel.height), middlePanelScroll, new Rect (0, 0, middlePanel.width + ((middleImage.width - middlePanel.width < 0 ? 0 : middleImage.width - middlePanel.width)), middlePanel.height + ((middleImage.height - middlePanel.height < 0 ? 0 : middleImage.height - middlePanel.height))));
									{
										middleView = new Rect (0, 0, tempTexture.width * currentZoomAmount, tempTexture.height * currentZoomAmount);
										middleImage = new Rect (middlePanel.width / 2 - ((tempTexture.width / 2) * currentZoomAmount), middlePanel.height / 2 - ((tempTexture.height / 2) * currentZoomAmount), tempTexture.width * currentZoomAmount, tempTexture.height * currentZoomAmount);
										//draw it
										//if()
										EditorGUI.DrawPreviewTexture (middleImage, tempTexture, usableMaterial);
										//if collisions, draw them
										if (showCollisions) {
											DrawCollisionEditorArea ();
										} else {
											setup_scene = false;
										}
									}
									GUI.EndScrollView ();
								}
								break;
							}
						}
					}
					GUI.EndGroup ();
				}
				#endregion
				#region right-panel
				if (currentObject != null) {
					GUI.Box (rightPanel, "");
					GUILayout.BeginArea (rightPanel);
					{
						if (currentType.Name == "SSprite") {
							DrawCurrentSpriteInfo ();
						} else if (currentType.Name == "SObject") {
							DrawCurrentObjectInfo ();
						}
					}
					GUILayout.EndArea ();
				}
				#endregion
				#region bottom-panel
				if (currentType != null && currentType.Name == "SObject") {
					GUI.Box (sobject_bottomPanel, "");
					GUI.BeginGroup (sobject_bottomPanel);
					{
						
					}
					GUI.EndGroup ();
				}
				#endregion
			}
		}

		void DrawToolStrip ()
		{
			if (GUILayout.Button ("Create", EditorStyles.toolbarDropDown)) {
				GenericMenu createMenu = new GenericMenu ();

				createMenu.AddItem (new GUIContent ("Sprite"), false, AssetCreation.Create_Sprite);
				createMenu.AddItem (new GUIContent ("Object"), false, AssetCreation.Create_Object);
				createMenu.AddItem (new GUIContent ("Room"), false, AssetCreation.Create_Room);
				createMenu.AddItem (new GUIContent ("Background"), false, AssetCreation.Create_Background);
				createMenu.AddItem (new GUIContent ("Timeline"), false, AssetCreation.Create_Timeline);
				createMenu.AddItem (new GUIContent ("Path"), false, AssetCreation.Create_Path);
			
				createMenu.DropDown (new Rect (0, 0, 0, 16));

				EditorGUIUtility.ExitGUI ();
			}
			GUILayout.FlexibleSpace ();
			/*if (currentType != null && currentType.Name == "SSprite")
				currentZoomAmount = EditorGUI.Slider (new Rect (205f, 0, 200f, 15f), currentZoomAmount, 0.01f, 30);*/
		}

		/// <summary>
		/// Load a sprite from the sprite panel
		/// </summary>
		/// <param name="s">The current SSprite.</param>
		void OnSpriteLoad (SSprite s)
		{
			currentType = typeof(SSprite);
			tempTexture = null;
			if (s.materialLocation != null) {
				tempMaterial = (Material)AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (s.materialLocation), typeof(Material));
				usableMaterial = tempMaterial;
				usableMaterial.color = s.textureColorAddition;
			} else {
				tempMaterial = null;
				usableMaterial = null;
			}
			if (s.textureLocation != null) {
				if (s.usesInternalTexture) {
					refTexture = (Texture2D)AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (s.textureLocation), typeof(Texture2D));	
					if (refTexture != null) {
						if (s.flipX && !s.flipY) {
							tempTexture = TextureEffects.FlipTextureX (refTexture.width, refTexture.height, ref refTexture);
						} else if (s.flipY && !s.flipX) {
							tempTexture = TextureEffects.FlipTextureY (refTexture.width, refTexture.height, ref refTexture);
						} else if (s.flipX && s.flipY) {
							tempTexture = TextureEffects.FlipTextureX (refTexture.width, refTexture.height, ref refTexture);
							tempTexture = TextureEffects.FlipTextureY (refTexture.width, refTexture.height, ref tempTexture);
						} else {
							tempTexture = refTexture;
						}
					}
				} else {
					if (File.Exists (s.textureLocation)) {
						byte[] bytes = File.ReadAllBytes (s.textureLocation);
						Texture2D cT = new Texture2D (0, 0);
						cT.LoadImage (bytes);
						tempTexture = cT;
					} else {
						tempTexture = null;
					}
				}
			} else {
				tempTexture = null;
			}
			currentObject = s;
		}

		void OnObjectLoad (SObject o)
		{
			tempMaterial = null;
			usableMaterial = null;

			currentType = typeof(SObject);
			currentObject = o;
		}

		/// <summary>
		/// Reload a sprite from OnInspectorUpdate
		/// </summary>
		void OnSpriteRefresh ()
		{
			SSprite s = (SSprite)currentObject;
			if (tempMaterial != null) {
				tempMaterial.color = s.textureColorAddition;
				var paths = AssetDatabase.FindAssets (tempMaterial.name);
				if (paths.Length > 0) {
					s.materialLocation = paths [0];
					usableMaterial = tempMaterial;
				}
			} else {
				s.materialLocation = null;
				usableMaterial = null;
			}
			if (refTexture != null) {
				var paths = AssetDatabase.FindAssets (refTexture.name);
				if (paths.Length > 0) {
					s.textureLocation = paths [0];
					if (s.flipX && !s.flipY) {
						tempTexture = TextureEffects.FlipTextureX (refTexture.width, refTexture.height, ref refTexture);
					} else if (s.flipY && !s.flipX) {
						tempTexture = TextureEffects.FlipTextureY (refTexture.width, refTexture.height, ref refTexture);
					} else if (s.flipX && s.flipY) {
						tempTexture = TextureEffects.FlipTextureX (refTexture.width, refTexture.height, ref refTexture);
						tempTexture = TextureEffects.FlipTextureY (refTexture.width, refTexture.height, ref tempTexture);
					} else {
						tempTexture = refTexture;
					}
				}
			} else {
				s.textureLocation = null;
			}

			EditorUtility.SetDirty (currentObject);
			Repaint ();
		}

		void OnInspectorUpdate ()
		{
			if (updateAllAssets) {
				updateAllAssets = false;
				ImportAssets ();
			}

			if (update) {
				//update information
				update = false;
				switch (currentType.Name) {
				case "SSprite":
					OnSpriteRefresh ();
					break;
				}
			}
		}

		/// <summary>
		/// Draws the current object info.
		/// </summary>
		void DrawCurrentSpriteInfo ()
		{
			rightPanelScroll = GUILayout.BeginScrollView (rightPanelScroll);
			{
				//draw the sprite name area
				SSprite s = (SSprite)currentObject;

				EditorGUI.BeginChangeCheck ();

				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Name:");
					EditorGUI.BeginChangeCheck ();
					s.name = GUILayout.TextField (s.name);
					if (EditorGUI.EndChangeCheck ()) {
						EditorUtility.SetDirty (currentObject);
					}
				}
				GUILayout.EndHorizontal ();
				//show 2 buttons, one to load the sprite and another to edit it
				GUILayout.Space (25);

				GUILayout.Label ("Image Settings", EditorStyles.boldLabel);

				s.usesInternalTexture = EditorGUILayout.ToggleLeft ("Uses Internal Texture", s.usesInternalTexture, GUILayout.Width (190f));
				if (s.usesInternalTexture) {
					refTexture = (Texture2D)EditorGUILayout.ObjectField (refTexture, typeof(Texture2D), false);
					if (GUILayout.Button ("Edit Texture")) {
							
					}
				} else {
					GUILayout.BeginVertical ();
					{
						GUILayout.BeginHorizontal ();
						{
							if (GUILayout.Button ("Load From Disc")) {
					
							}
							if (GUILayout.Button ("Edit")) {

							}
						}
						GUILayout.EndHorizontal ();
						if (GUILayout.Button ("Update External Texture")) {
								
						}
					}
					GUILayout.EndVertical ();
				}

				GUILayout.Space (25);

				GUILayout.Label ("Sprite Settings", EditorStyles.boldLabel);

				s.textureColorAddition = EditorGUILayout.ColorField ("Main Color:", s.textureColorAddition, GUILayout.Width (190f));

				GUILayout.BeginHorizontal ();
				{
					s.flipX = EditorGUILayout.ToggleLeft ("Flip X:", s.flipX, GUILayout.Width (90));
					s.flipY = EditorGUILayout.ToggleLeft ("Flip Y:", s.flipY, GUILayout.Width (90));
				}
				GUILayout.EndHorizontal ();

				tempMaterial = (Material)EditorGUILayout.ObjectField (tempMaterial, typeof(Material), false);
				if (GUILayout.Button ("Refresh Sprite")) {
					update = true;
				}
				s.currentDepth = EditorGUILayout.IntField ("Z Depth", s.currentDepth, GUILayout.Width (190));

				GUILayout.Space (25);

				GUILayout.Label ("Collision Settings", EditorStyles.boldLabel);

				GUILayout.BeginHorizontal ();
				{
					GUILayout.Label ("Collider Type");
					s.collisions.colliderType = (SCollisions.ColliderType)EditorGUILayout.EnumPopup (s.collisions.colliderType, GUILayout.Width (100));
				}
				GUILayout.EndHorizontal ();
				if (GUILayout.Button ("Edit Collision Mask")) {
					showCollisions = true;
				}
				if (GUILayout.Button ("Clear Collision Mask")) {
					pointsForLines.Clear ();
				}
				GUILayout.Label ("Nodes:" + pointsForLines.Count);
				/*GUILayout.Label ("Pos:" + pointsForLines [0]);*/

				if (EditorGUI.EndChangeCheck ()) {
					EditorUtility.SetDirty (currentObject);
				}
			}
			GUILayout.EndScrollView ();
		}

		void DrawCurrentObjectInfo ()
		{
			rightPanelScroll = GUILayout.BeginScrollView (rightPanelScroll);
			{
				//draw the sprite name area
				SObject o = (SObject)currentObject;
			}
			GUILayout.EndScrollView ();
		}

		#region Collision Masks

		public List<Vector2> gridPoints = new List<Vector2> ();
		public bool setup_scene = false;
		public bool inEditMode = false;
		public float _gridSpacing = 20;
		public List<Vector2> pointsForLines = new List<Vector2> ();

		public void CreateGridPoints ()
		{
			gridPoints.Clear ();

			for (int x = 0; x <= refTexture.width; x++) {
				float _posX = ((float)(middleImage.width * currentZoomAmount) / (float)(refTexture.width * currentZoomAmount)) * x;
				for (int y = 0; y <= refTexture.height; y++) {
					float _posY = ((float)(middleImage.height * currentZoomAmount) / (float)(refTexture.height * currentZoomAmount)) * y;
					gridPoints.Add (new Vector2 (_posX, _posY));
				}
			}
		}

		void DrawCollisionEditorArea ()
		{
			if (GUI.Button (new Rect (0, 0, 100, 20), "Save")) {
				showCollisions = false;
			}
			inEditMode = EditorGUI.ToggleLeft (new Rect (0, 50f, 50f, 20f), "Edit:", inEditMode);

			GUI.BeginGroup (new Rect (middleImage.x, middleImage.y, middleImage.width + 1, middleImage.height + 1));
			{

				//check for mouse input
				Event e = Event.current;
				Vector2 mousePos = e.mousePosition;

				float currentX = ReturnNearestMultiple (mousePos.x);
				float currentY = ReturnNearestMultiple (mousePos.y);

				if (inEditMode) {

					//Debug.Log ("Current Event: " + e.type);
					DrawGrid ();
					if (pointsForLines.Count > 0)
						DrawLines ();

					if (e.type == EventType.repaint) {
						//snap the box
						if (gridPoints.Contains (new Vector2 (currentX, currentY))) {
							EditorGUI.DrawRect (new Rect (currentX - 5, currentY - 5, 10, 10), Color.red);	
						}
						e.Use ();
					} else if (e.type == EventType.mouseUp) {
						//add a new point
						//if (!pointsForLines.Contains (new Vector2 (currentX, currentY))) {
						pointsForLines.Add (new Vector2 (currentX, currentY));
						e.Use ();
						//}
					}
				} else {
					if (e.type == EventType.mouseUp) {
						if (gridPoints.Contains (new Vector2 (currentX, currentY))) {
							//delete the point
							pointsForLines.Remove (new Vector2 (currentX, currentY));
							e.Use ();
						}
					}
						
					DrawGrid ();
					if (pointsForLines.Count > 0)
						DrawLines ();
				}
			}
			GUI.EndGroup ();
		}

		public void DrawGrid ()
		{
			if (!setup_scene) {
				setup_scene = true;
				CreateGridPoints ();
			} else {
				for (int x = 0; x <= refTexture.width; x++) {
					float _posX = ((float)(middleImage.width * currentZoomAmount) / (float)(refTexture.width * currentZoomAmount)) * x;
					EditorGUI.DrawRect (new Rect (_posX, 0, 1, refTexture.height * currentZoomAmount), new Color (1, 1, 1, 0.3f));
				}
				for (int y = 0; y <= refTexture.height; y++) {
					float _posY = ((float)(middleImage.height * currentZoomAmount) / (float)(refTexture.height * currentZoomAmount)) * y;
					EditorGUI.DrawRect (new Rect (0, _posY, refTexture.height * currentZoomAmount, 1), new Color (1, 1, 1, 0.3f));
				}
			}
		}

		void DrawLines ()
		{
			/*for (int x = 0; x <= refTexture.width; x++) {
				float _posX = ((float)(middleImage.width * currentZoomAmount) / (float)(refTexture.width * currentZoomAmount)) * x;
				EditorGUI.DrawRect (new Rect (_posX - 5, 0 - 5, 10, 10), new Color (0, 1, 1, 0.3f));
				for (int y = 0; y <= refTexture.height; y++) {
					float _posY = ((float)(middleImage.height * currentZoomAmount) / (float)(refTexture.height * currentZoomAmount)) * y;
					//Debug.Log (_posX + "," + _posY);
					EditorGUI.DrawRect (new Rect (_posX - 5, _posY - 5, 10, 10), new Color (0, 1, 1, 0.3f));
				}
			}*/

		}

		public float ReturnNearestMultiple (float f)
		{
			//generate a list of multiples
			List<float> list = new List<float> ();
			GenerateMultiples (ref list);

			//go through each and see which one is closest
			float closeValue = 0;
			float lastCloseness = 1000;
			foreach (float lV in list) {
				if (Mathf.Abs (lV - f) < lastCloseness) {
					lastCloseness = Mathf.Abs (lV - f);
					closeValue = lV;
				}
			}

			return closeValue;
		}

		public void GenerateMultiples (ref List<float> list)
		{
			for (int i = 0; i <= refTexture.width; i++) {
				float y = ((float)(middleImage.height * currentZoomAmount) / (float)(refTexture.height * currentZoomAmount)) * i;
				list.Add (y);
			}
		}

		#endregion

	}

}