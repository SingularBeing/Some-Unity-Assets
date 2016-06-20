using UnityEngine;
using UnityEditor;
using System.Collections;
using CYRO;
using System.Collections.Generic;

namespace CYRO
{
	[CustomEditor (typeof(SceneObj))]
	public class SceneObjEditorCollisions : Editor
	{
		SceneObj me;
		public Transform handleTransform;
		public Quaternion handleRotation;
		public const float handleSize = 0.04f, pickSize = 0.06f;
		public int selectedIndex = -1;

		private void OnSceneGUI ()
		{
			me = (SceneObj)target;

			handleTransform = me.transform;
			handleRotation = Tools.pivotRotation == PivotRotation.Local ?
				handleTransform.rotation : Quaternion.identity;

			for (int i = 0; i < me.points.Count; i++) {
				Vector3 p0 = ShowPoint (i);
				if (i + 1 < me.points.Count) {
					Vector3 p1 = ShowPoint (i + 1);
					Handles.color = Color.white;
					Handles.DrawLine (p0, p1);
				}
			}
		}

		private Vector3 ShowPoint (int index)
		{
			Vector3 point = handleTransform.TransformPoint (me.points [index]);

			Handles.color = Color.blue;
			if (Handles.Button (point, handleRotation, handleSize, pickSize, Handles.DotCap)) {
				selectedIndex = index;
			}
			if (selectedIndex == index) {
				EditorGUI.BeginChangeCheck ();
				point = Handles.DoPositionHandle (point, handleRotation);
				if (EditorGUI.EndChangeCheck ()) {

				}
			}

			return point;
		}
	}

}