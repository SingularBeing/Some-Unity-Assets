using UnityEngine;
using UnityEditor;
using System.Collections;
using CYRO;
using System;

namespace CYRO
{

	public class E_SpriteEditorWindow : EditorWindow
	{

		//static
		public static E_SpriteEditorWindow window;
		public static SSprite current;
		//non-static
		public string spriteName;
		//gets
		public Vector2 WindowSize {
			get {
				return window.position.size;
			}
		}

		Rect leftPanel, middlePanel, rightPanel;

		public static void Open (SSprite sprite)
		{
			Type[] types = new Type[]{ typeof(E_MainWindow) };
			current = sprite;
			window = (E_SpriteEditorWindow)GetWindow<E_SpriteEditorWindow> (current.name, true, types);
			window.spriteName = current.name;
		}

		/// <summary>
		/// Upon the start of the window
		/// </summary>
		void OnEnable ()
		{
			
		}

		/// <summary>
		/// When the window is closing
		/// </summary>
		void OnDestroy ()
		{
			
		}

		void SetRects ()
		{
			leftPanel = new Rect (0, 0, 175f, WindowSize.y);
			middlePanel = new Rect (leftPanel.width + 5f, 0, leftPanel.width, WindowSize.y);
			rightPanel = new Rect (leftPanel.width + middlePanel.width + 5f, 0, WindowSize.x - leftPanel.width - middlePanel.width - 5f, WindowSize.y);
		}

		void OnGUI ()
		{
			SetRects ();
			//left rect
			#region left rect
			GUILayout.BeginArea (leftPanel, "box");
			{
				spriteName = EditorGUILayout.TextField (new GUIContent ("Name"), spriteName);
			}
			GUILayout.EndArea ();
			#endregion
		}

	}

}