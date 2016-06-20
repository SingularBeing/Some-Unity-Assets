using UnityEngine;
using System.Collections;
using UnityEditor;
using CYRO.EditorWindowVisualEditor;
using System.Collections.Generic;

namespace CYRO.EditorWindowVisualEditor
{

	public class EditorWindowCreation : EditorWindow
	{

		public List<GUIElement_Base> allElements = new List<GUIElement_Base> ();
		public GUIElement_Base objectSelected = null;
		public float scaleXAddition, scaleYAddition;
		public Rect resizeArea;
		public string currentText;
		static bool showObjectBackdrop = true;
		static bool objectSnapToOtherObjects = false;
		static bool objectSnapInRange = false;
		static bool objectSnapLeft, objectSnapRight, objectSnapTop, objectSnapDown;
		static Rect currentSnapObjectRect;

		[MenuItem ("Window/EditorWindow Creation")]
		public static void Open ()
		{
			RecoverDataFromSaving ();
			GetWindow (typeof(EditorWindowCreation), false, "Window Creation");
		}

		bool sampleBool = false;
		bool buttonActive = false, buttonActive2;
		bool currentlyDragging = false;
		int currentDragIcon = -1;
		bool editModeActive = false;
		bool canSnap = true;

		void OnEnable ()
		{
		}

		void OnGUI ()
		{
			#region Toolbar
			GUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.Width (this.position.size.x));
			{
				DrawLeftTopToolbar ();
				DrawMiddleTopToolbar ();
			}
			GUILayout.EndHorizontal ();
			#endregion
			#region Object Panel
			GUILayout.BeginArea (new Rect (0, 20, 175, this.position.size.y - 20));
			{
				GUI.Box (new Rect (0, 0, 175, this.position.size.y - 20f), "");
				foreach (GUIElement_Base b in allElements) {
					GUI.backgroundColor = IsSelected (b.myName) ? Color.blue : Color.white;
					if (GUILayout.Button (b.myName, Utility.GetGUIStyle ("ObjectPickerResultsEven"), GUILayout.Width (175f))) {
						objectSelected = b;
						ResetResize ();
					}
					GUI.backgroundColor = Color.white;
				}
			}
			GUILayout.EndArea ();
			#endregion
			#region Properties Panel
			GUILayout.BeginArea (new Rect (this.position.size.x - 175, 20, 175, this.position.size.y - 20));
			{
				GUI.Box (new Rect (0, 0, 175, this.position.size.y - 20), "");
				if (objectSelected != null) {
					//name
					objectSelected.myName = EditorGUILayout.TextField (objectSelected.myName, Utility.GetGUIStyle ("LargeTextField"));
					//title
					GUILayout.Space (10);
					EditorGUI.BeginChangeCheck ();
					objectSelected.myRect = EditorGUILayout.RectField ("Position", objectSelected.myRect);
					if (EditorGUI.EndChangeCheck ()) {
						ResetResize ();
					}
				}
			}
			GUILayout.EndArea ();
			#endregion
			#region CurrentTool bar
			//current tool bar
			GUILayout.BeginArea (new Rect (175f, 20f, this.position.size.x - 175f, 30));
			{
				GUI.Box (new Rect (0, 0, this.position.size.x - (175 * 2), 30), "", Utility.GetGUIStyle ("GroupBox"));
			}
			GUILayout.EndArea ();
			#endregion
			#region Middle Area
			GUILayout.BeginArea (new Rect (175, 50, this.position.size.x - (175 * 2), this.position.size.y - 50));
			{
				//draw each element
				foreach (GUIElement_Base b in allElements) {
					if (b.myIndentifier.Contains ("GUI.")) {
						//check for object
						if (b.myIndentifier == "GUI.Label") {
							if (showObjectBackdrop)
								GUI.Box (b.myRect, "", Utility.GetGUIStyle ("Wizard Box"));
							//draw a label
							if (!editModeActive)
								GUI.Label (b.myRect, b.myContent);
							if (editModeActive && objectSelected != null && objectSelected.myName != b.myName)
								GUI.Label (b.myRect, b.myContent);
						} else if (b.myIndentifier == "GUI.Button") {
							//draw a button
							GUI.enabled = false;
							GUI.Button (b.myRect, b.myContent);
							GUI.enabled = true;
						} else if (b.myIndentifier == "GUI.Box") {
							GUI.Box (b.myRect, b.myContent);
						} else if (b.myIndentifier == "GUI.Texture") {
							
						}
					} else if (b.myIndentifier.Contains ("GUILayout.")) {
						
					}
				}
			}
			GUILayout.EndArea ();
			#endregion
			#region Draw Handles
			if (objectSelected != null) {
				Rect accurateRect = new Rect (objectSelected.myRect.x + 175, objectSelected.myRect.y + 50, objectSelected.myRect.width, objectSelected.myRect.height);
				GUI.Box (resizeArea, "", Utility.GetGUIStyle ("RL Background"));
				//if (objectSelected.myIndentifier != "GUI.Box") {
				if (!editModeActive) {
					if (GUI.Button (new Rect (objectSelected.myRect.x + objectSelected.myRect.width + 175f, objectSelected.myRect.y + 50f, 38, 20), "Edit")) {
						editModeActive = true;
					}
				} else {
					if (GUI.Button (new Rect (objectSelected.myRect.x + objectSelected.myRect.width + 175f, objectSelected.myRect.y + 50f, 38, 20), "Done")) {
						EditorGUIUtility.editingTextField = false;
						editModeActive = false;
					}
					objectSelected.myContent = new GUIContent (EditorGUI.TextArea (accurateRect, objectSelected.myContent.text));
				}
				//}
				//draw the snap options
				if (objectSnapToOtherObjects) {
					//check for the position
					foreach (GUIElement_Base b in allElements) {
						if (b != null && b.myName != objectSelected.myName) {
							//Debug.Log (Mathf.Abs ((objectSelected.myRect.x + objectSelected.myRect.width) - (b.myRect.x + b.myRect.width)));
							//Debug.Log (Mathf.Abs (objectSelected.myRect.x - (b.myRect.x + b.myRect.width)));
							if (Mathf.Abs (objectSelected.myRect.x - b.myRect.x) <= 1.3f) {
								//display a line
								if (canSnap) {
									objectSnapLeft = true;
									EditorGUI.DrawRect (new Rect (175f + objectSelected.myRect.x, 0, 1, this.position.size.y), Color.green);
									currentSnapObjectRect = b.myRect;
									objectSnapInRange = true;
								}
							} else if (Mathf.Abs (objectSelected.myRect.x - (b.myRect.x + b.myRect.width)) <= 3f) {
								//display a line
								if (canSnap) {
									objectSnapRight = true;
									EditorGUI.DrawRect (new Rect (objectSelected.myRect.x + 175f, 0, 1, this.position.size.y), Color.green);
									currentSnapObjectRect = b.myRect;
									objectSnapInRange = true;
								}
							}
						}
					}
				}
			}
			#endregion
			#region Input
			Event e = Event.current;
			Vector2 mousePosition = e.mousePosition;
			if (e.button == 0) {
				if (e.isMouse && e.type == EventType.mouseUp) {
					if (currentDragIcon == -1) {
						//check if we are in an objects position
						foreach (GUIElement_Base b in allElements) {
							Rect accurateRect = new Rect (b.myRect.x + 175, b.myRect.y + 50, b.myRect.width, b.myRect.height);
							if (accurateRect.Contains (new Vector2 (mousePosition.x, mousePosition.y))) {
								objectSelected = null;
								objectSelected = b;
								//Debug.Log (objectSelected.myName);
								ResetResize ();
							}
						}
					}
					//Reset info
					currentDragIcon = -1;
					if (objectSelected != null) {
						objectSnapInRange = false;
						canSnap = false;
						objectSnapRight = false;
						objectSnapLeft = false;
					}
					e.Use ();
				} else if (e.isMouse && e.type != EventType.mouseUp) {
					if (objectSelected != null) {
						Rect accurateRect = new Rect (objectSelected.myRect.x + 175, objectSelected.myRect.y + 50, objectSelected.myRect.width, objectSelected.myRect.height);
						if (!editModeActive) {
							if (accurateRect.Contains (mousePosition) && (currentDragIcon == -1 || currentDragIcon == 2)) {
								currentDragIcon = 2;
							}
						}
						if (resizeArea.Contains (mousePosition) && (currentDragIcon == -1 || currentDragIcon == 1)) {
							currentDragIcon = 1;
						}

						if (currentDragIcon == 1) {
							resizeArea = new Rect (mousePosition.x, mousePosition.y, resizeArea.width, resizeArea.height);
							//change the rect of the selected object
							objectSelected.myRect = new Rect (objectSelected.myRect.x, objectSelected.myRect.y, mousePosition.x - objectSelected.myRect.x - 150f, mousePosition.y - objectSelected.myRect.y - 50);
						}
						if (currentDragIcon == 2) {
							if (objectSnapInRange) {
								if (objectSnapToOtherObjects) {
									if (objectSnapLeft) {
										Rect extendedRect = new Rect (objectSelected.myRect.x + 165, objectSelected.myRect.y + 25, objectSelected.myRect.width + 25f, objectSelected.myRect.height + 25f);
										if (extendedRect.Contains (mousePosition)) {
											objectSelected.myRect = new Rect (currentSnapObjectRect.x, mousePosition.y - 50f - (objectSelected.myRect.height / 2), objectSelected.myRect.width, objectSelected.myRect.height);
										} else {
											objectSnapInRange = false;
											objectSnapLeft = false;
											canSnap = false;
											objectSelected.myRect = new Rect (mousePosition.x - 175f - (objectSelected.myRect.width / 2), mousePosition.y - 50f - (objectSelected.myRect.height / 2), objectSelected.myRect.width, objectSelected.myRect.height);
										}
									} else if (objectSnapRight) {
										Rect extendedRect = new Rect (objectSelected.myRect.x + 165, objectSelected.myRect.y + 25, objectSelected.myRect.width + 25f, objectSelected.myRect.height + 25f);
										if (extendedRect.Contains (mousePosition)) {
											objectSelected.myRect = new Rect (currentSnapObjectRect.x + currentSnapObjectRect.width, mousePosition.y - 50f - (objectSelected.myRect.height / 2), objectSelected.myRect.width, objectSelected.myRect.height);
										} else {
											objectSnapInRange = false;
											objectSnapRight = false;
											canSnap = false;
											objectSelected.myRect = new Rect (mousePosition.x - 175f - (objectSelected.myRect.width / 2), mousePosition.y - 50f - (objectSelected.myRect.height / 2), objectSelected.myRect.width, objectSelected.myRect.height);
										}
									}
								}
							} else {
								objectSelected.myRect = new Rect (mousePosition.x - 175f - (objectSelected.myRect.width / 2), mousePosition.y - 50f - (objectSelected.myRect.height / 2), objectSelected.myRect.width, objectSelected.myRect.height);
								canSnap = true;
							}
							ResetResize ();
						}
						e.Use ();
					}
				}
			}
			#endregion
		}

		void DrawLeftTopToolbar ()
		{
			if (GUILayout.Button ("Create", EditorStyles.toolbarDropDown, GUILayout.Width (65))) {
				GenericMenu createMenu = new GenericMenu ();

				createMenu.AddItem (new GUIContent ("GUI/Display/Label"), false, MenuItemJob, "GUI.Label");
				createMenu.AddItem (new GUIContent ("GUI/Display/Box"), false, MenuItemJob, "GUI.Box");
				createMenu.AddItem (new GUIContent ("GUI/Display/Texture"), false, MenuItemJob, "GUI.Texture");
				createMenu.AddItem (new GUIContent ("GUI/Interactable/Button"), false, MenuItemJob, "GUI.Button");
				createMenu.AddItem (new GUIContent ("GUI/Interactable/Repeat Button"), false, MenuItemJob, "GUI.RepeatButton");
				createMenu.AddItem (new GUIContent ("GUI/Interactable/Toggle (Left)"), false, MenuItemJob, "GUI.ToggleLeft");
				createMenu.AddItem (new GUIContent ("GUI/Interactable/Toggle (Right)"), false, MenuItemJob, "GUI.ToggleRight");
				createMenu.AddItem (new GUIContent ("GUI/Input/Text Field"), false, MenuItemJob, "GUI.TextField");
				createMenu.AddItem (new GUIContent ("GUI/Input/Text Area"), false, MenuItemJob, "GUI.TextArea");
				createMenu.AddItem (new GUIContent ("GUI/Input/Password Field"), false, MenuItemJob, "GUI.PasswordField");

				createMenu.DropDown (new Rect (0, 0, 0, 16));

				EditorGUIUtility.ExitGUI ();
			}
			GUILayout.Space (6);
			if (GUILayout.Button ("Refresh", EditorStyles.toolbarButton, GUILayout.Width (65))) {
				allElements.Clear ();
				objectSelected = null;
				allElements = Utility.SplitTxtFile (Utility.LoadEditorWindowFromFile ());
			}
			//GUILayout.FlexibleSpace ();
		}

		void DrawMiddleTopToolbar ()
		{
			GUILayout.Space (35);
			if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (50))) {
				GenericMenu createMenu = new GenericMenu ();

				createMenu.AddItem (new GUIContent ("New"), false, MenuItemJob, "New File");
				createMenu.AddItem (new GUIContent ("Open"), false, MenuItemJob, "Open File");
				createMenu.AddItem (new GUIContent ("Save"), false, MenuItemJob, "Save File");
				createMenu.AddItem (new GUIContent ("Save As"), false, MenuItemJob, "Save File As");

				createMenu.DropDown (new Rect (175, 0, 0, 16));

				EditorGUIUtility.ExitGUI ();
			}
			GUILayout.Space (6);
			if (GUILayout.Button ("View", EditorStyles.toolbarDropDown, GUILayout.Width (50))) {
				GenericMenu createMenu = new GenericMenu ();

				createMenu.AddItem (new GUIContent ("Show Grid"), false, MenuItemJob, "Show Grid");
				createMenu.AddItem (new GUIContent ("Show Object Backdrop"), showObjectBackdrop, MenuItemJob, "Show Object Backdrop");

				createMenu.AddSeparator ("");

				createMenu.AddItem (new GUIContent ("Snapping/Snap to Other Objects"), objectSnapToOtherObjects, MenuItemJob, "Snap Other Object");

				createMenu.DropDown (new Rect (175 + 50 + 6, 0, 0, 16));

				EditorGUIUtility.ExitGUI ();
			}
			//GUILayout.FlexibleSpace ();
		}

		void MenuItemJob (object type)
		{
			if (type == "Show Object Backdrop") {
				GenericSaving.SetString ("ObjBackdrop", showObjectBackdrop == true ? "false" : "true");
			} else if (type == "Snap Other Object") {
				GenericSaving.SetString ("SnapToObject", objectSnapToOtherObjects == true ? "false" : "true");
			} else if (type == "GUI.Label") {
				GUIElement_Base b = new GUIElement_Base ();
				b.myName = "Label(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.Label";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.Button") {
				GUIElement_Linkable b = new GUIElement_Linkable ();
				b.myName = "Button(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.Button";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.RepeatButton") {
				GUIElement_Linkable b = new GUIElement_Linkable ();
				b.myName = "Repeat Button(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.RepeatButton";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.TextField") {
				GUIElement_TextInput b = new GUIElement_TextInput ();
				b.myName = "Text Field(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.TextField";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.TextArea") {
				GUIElement_TextInput b = new GUIElement_TextInput ();
				b.myName = "Text Area(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.TextArea";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.ToggleRight") {
				GUIElement_Boolean b = new GUIElement_Boolean ();
				b.myName = "Toggle Right(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.ToggleRight";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.ToggleLeft") {
				GUIElement_Boolean b = new GUIElement_Boolean ();
				b.myName = "Toggle Left(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.ToggleLeft";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.PasswordField") {
				GUIElement_TextInput b = new GUIElement_TextInput ();
				b.myName = "Password Field(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.PasswordField";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.Box") {
				GUIElement_Display b = new GUIElement_Display ();
				b.myName = "Box(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.Box";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			} else if (type == "GUI.Texture") {
				GUIElement_Display b = new GUIElement_Display ();
				b.myName = "Texture(" + Random.Range (0, int.MaxValue) + ")";
				b.myContent = new GUIContent ("[Insert Content]");
				b.myIndentifier = "GUI.Texture";
				b.myRect = new Rect (0, 0, 120, 20);
				allElements.Add (b);
				objectSelected = allElements [allElements.Count - 1];
			}

			RecoverDataFromSaving ();
		}

		static void RecoverDataFromSaving ()
		{
			EditorWindowCreation.showObjectBackdrop = GenericSaving.GetBool ("ObjBackdrop");
			EditorWindowCreation.objectSnapToOtherObjects = GenericSaving.GetBool ("SnapToObject");
		}

		bool IsSelected (string name)
		{
			if (objectSelected != null) {
				if (objectSelected.myName == name) {
					return true;
				}
			}

			return false;
		}

		void ResetResize ()
		{
			resizeArea = new Rect (objectSelected.myRect.x + objectSelected.myRect.width + 150f + 10f, objectSelected.myRect.y + objectSelected.myRect.height + 50f, 20, 20);
		}

	}

}