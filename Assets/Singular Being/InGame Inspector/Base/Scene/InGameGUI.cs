// Script created by: Andrew Burke (_Demetrix)
//
// One Voxel © 2016
#if UNITY_EDITOR
using System;
using CYRO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace CYRO
{
	public class InGameGUI : MonoBehaviour
	{
		public Color _hierarchyBackgroundColor = new Color (0, 0, 0, 0.8f), _inspectorBackgroundColor = Color.black, _inspectorTopBarColor = Color.blue, _inspectorSideColor = Color.red, _inspectorComponentBackColor = Color.green;
		public float _hierarchyWidth = 175f, _inspectorWidth = 175f;

		private GameObject _selectedGameObject;
		private GameObject _topArrayParentObject;

		private static string _topParentTag = "CYRO_TopParent";

		private Vector2 scrollView = Vector2.zero;

		private List<Transform> expandedObjects = new List<Transform> ();

		void Start ()
		{
			_topArrayParentObject = GameObject.FindGameObjectWithTag (_topParentTag);
			if (_topArrayParentObject == null) {
				Debug.LogError ("There is no Master parent object which contains every object in the scene with the tag: " + _topParentTag + ". Please assign this.");
				return;
			}
		}

		public Transform[] GetAllObjectsFromScene ()
		{

			List<Transform> allTransforms = new List<Transform> ();
			List<Transform> wentThrough = new List<Transform> ();
			List<Transform> currentTransforms = new List<Transform> ();
			allTransforms.Add (_topArrayParentObject.transform);

			int amountOfChildren = 0;
			int helpMe = 0;
			while (true) {

				if (helpMe >= 25) {
					//Debug.Log ("Helped out");
					break;
				}

				foreach (Transform t in allTransforms) {
					if (!wentThrough.Contains (t)) {
						int childCount = t.childCount;
						if (childCount == 0)
							continue;
						for (int i = 0; i < childCount; i++) {
							currentTransforms.Add (t.GetChild (i));
						}
						wentThrough.Add (t);
					}
				}

				//Debug.Log ("Current Transforms Count: " + currentTransforms.Count + ", Help Count: " + helpMe);

				helpMe++;

				if (currentTransforms.Count == 0)
					break;
				allTransforms.AddRange (currentTransforms);
				currentTransforms.Clear ();

			}

			//Debug.Log ("Done mate");
			//Debug.Log ("Report: " + allTransforms.Count);
			return allTransforms.ToArray ();
		}

		public string currentSearch = "";

		void OnGUI ()
		{
			HierarchyBackground background = null;
			#region Hierarchy
			//draw the toolbar
			//draw the background
			background = new HierarchyBackground (new Rect (0, 0, _hierarchyWidth, Screen.height), _inspectorBackgroundColor);
			background = new HierarchyBackground (new Rect (_hierarchyWidth, 0, 5f, Screen.height), _inspectorSideColor);
			GUILayout.BeginHorizontal (EditorStyles.toolbar);
			{
				//display a search
				currentSearch = GUILayout.TextField (currentSearch, EditorStyles.toolbarTextField, GUILayout.Width (_hierarchyWidth - 20f - 40f));
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Search", EditorStyles.toolbarButton)) {
					
				}
			}
			GUILayout.EndHorizontal ();
			//draw the objects onto the hierarchy
			GUILayout.BeginArea (new Rect (0, 20f, _hierarchyWidth, Screen.height - 20));
			{
				EditorGUILayout.BeginHorizontal ();
				{
					scrollView = EditorGUILayout.BeginScrollView (scrollView);
					{
						Transform[] transforms = GetAllObjectsFromScene ();

						foreach (Transform t in transforms) {
							//start with objects without any parents
							if (t.parent == null) {
								//show the object and it's children
								ShowObject (t, transforms);
							}
						}
					}
					EditorGUILayout.EndScrollView ();
				}
				EditorGUILayout.EndHorizontal ();
			}
			GUILayout.EndArea ();
			#endregion
			#region Inspector
			//draw the background
			background = new HierarchyBackground (new Rect (Screen.width - _inspectorWidth, 0, _inspectorWidth, Screen.height), _inspectorBackgroundColor);
			background = new HierarchyBackground (new Rect (Screen.width - _inspectorWidth - 5f, 0, 5f, Screen.height), _inspectorSideColor);
			if (_selectedGameObject != null) {
				GUI.BeginGroup (new Rect (Screen.width - _inspectorWidth, 0, _inspectorWidth, Screen.height));
				{
					InspectorObjectHeader.Init (new Rect (0, 0, _inspectorWidth, 30), _selectedGameObject, _inspectorTopBarColor, this);

					MonoBehaviour[] components = _selectedGameObject.GetComponents<MonoBehaviour> ();

					InspectorTransformComponent.Init (new Rect (5f, 40f, _inspectorWidth - 10f, 100f), _selectedGameObject.transform, _inspectorComponentBackColor);

					for (int i = 0; i < components.Length; i++) {
						InspectorComponent.Init (new Rect (10f, 160f + i * 50f, _inspectorWidth - 20f, 30f), components [i], _inspectorComponentBackColor);
					}
				}
				GUI.EndGroup ();
			}
			#endregion
		}

		GUIStyle GetGUIStyle (string nameOf)
		{
			foreach (GUIStyle style in GUI.skin.customStyles) {
				if (style.name == nameOf) {
					return style;
				}
			}

			return null;
		}

		int CheckForObjectsWithParent (Transform parent, Transform[] transforms)
		{
			int amountOfObjects = 0;
			foreach (Transform t in transforms) {
				//find children of the parent gameObject
				if (t.parent == parent.transform) {
					amountOfObjects++;
				}
			}

			return amountOfObjects;
		}

		void ShowObject (Transform parent, Transform[] transforms)
		{
			//show entry for parent object if it has objects
			if (CheckForObjectsWithParent (parent, transforms) > 0) {
				bool shown = false;
				GUILayout.BeginHorizontal ();
				{
					GUI.backgroundColor = Color.white;

					if (GUILayout.Button (IsEntryOpen (parent) ? "-" : "+", EditorStyles.miniButtonLeft, GUILayout.Width (20), GUILayout.Height (20))) {
						
						if (!expandedObjects.Contains (parent)) {
							expandedObjects.Add (parent);
							shown = true;
						}
						if (expandedObjects.Contains (parent) && shown == false) {
							expandedObjects.Remove (parent);
							shown = true;
						}
					}
					//GUILayout.Space (10);
					GUI.backgroundColor = parent.gameObject.activeInHierarchy == true ? Color.green : Color.red;
					if (GUILayout.Button (parent.name, EditorStyles.miniButtonRight, GUILayout.Height (20))) {
						_selectedGameObject = parent.gameObject;
					}
					GUI.backgroundColor = Color.white;
				}
				GUILayout.EndHorizontal ();

				if (IsEntryOpen (parent)) {
					//Debug.Log ("Keyboard: " + EditorGUIUtility.keyboardControl + ",  Hot: " + EditorGUIUtility.hotControl + ",  Current Variable:" + controlID);
					GUILayout.BeginHorizontal ();
					{
						GUILayout.Space (20);
						GUILayout.BeginVertical ();
						{
							foreach (Transform t in transforms) {
								//find children of the parent gameObject
								if (t.parent == parent.transform) {
									GUI.backgroundColor = t.gameObject.activeInHierarchy == true ? Color.green : Color.red;
									ShowObject (t, transforms);
									GUI.backgroundColor = Color.white;
								}
							}
						}
						GUILayout.EndVertical ();
					}
					GUILayout.EndHorizontal ();
				} 

				//EditorGUIUtility.hierarchyMode = false;
			} else {
				//display a label
				GUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button (parent.name, EditorStyles.miniButtonRight, GUILayout.Width (_hierarchyWidth / 2f))) {
						_selectedGameObject = parent.gameObject;
					}
				}
				GUILayout.EndHorizontal ();
			}
		}

		bool IsEntryOpen (Transform parent)
		{
			if (expandedObjects.Contains (parent)) {
				return true;
			}

			return false;
		}
	}
}

/*HierarchyGameObject _button = new HierarchyGameObject (new Rect (0, 20 * i, 100f, 15f), Color.white, Color.red, _allChildren [i].gameObject);
if (_button.isActive) {
	//do something

}*/
#endif