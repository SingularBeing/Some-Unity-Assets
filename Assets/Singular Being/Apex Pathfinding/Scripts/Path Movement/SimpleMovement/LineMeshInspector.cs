using UnityEngine;
using System.Collections;
using UnityEditor;

//using System;
//using System.IO;
//using System.Xml;
//using System.Collections.Generic;

/*
	[Script Info]

*/
[CustomEditor (typeof(LineMesh))]
public class LineMeshInspector : Editor
{

	private LineMesh mesh;
	private Transform handleTransform;
	private Quaternion handleRotation;
	private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	private int selectedIndex = -1;

	public override void OnInspectorGUI ()
	{
		mesh = (LineMesh)target;
		if (selectedIndex >= 0 && selectedIndex < mesh.ControlPointCount) {
			DrawSelectedPointInspector ();
		}
		if (GUILayout.Button ("Add Line")) {
			Undo.RecordObject (mesh, "Add Line");
			mesh.AddLine ();
			EditorUtility.SetDirty (mesh);
		}
	}

	private void DrawSelectedPointInspector ()
	{
		GUILayout.Label ("Selected Point");
		EditorGUI.BeginChangeCheck ();
		Vector3 point = EditorGUILayout.Vector3Field ("Position", mesh.GetPoint (selectedIndex));
		if (EditorGUI.EndChangeCheck ()) {
			Undo.RecordObject (mesh, "Move Point");
			EditorUtility.SetDirty (mesh);
			mesh.SetPoint (selectedIndex, point);
		}
	}

	private void OnSceneGUI ()
	{
		mesh = (LineMesh)target;
		handleTransform = mesh.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		Handles.color = Color.grey;
		for (int i = 0; i < mesh.ControlPointCount; i += 2) {
			Vector3 p0 = ShowPoint (i);
			Vector3 p1 = ShowPoint (i + 1);
			if (i + 2 < mesh.ControlPointCount) {
				Vector3 p2 = ShowPoint (i + 2);
				Handles.DrawLine (p1, p2);	
			}
			Handles.DrawLine (p0, p1);	
		}
	}

	private Vector3 ShowPoint (int index)
	{
		Vector3 point = handleTransform.TransformPoint (mesh.GetPoint (index));
		float size = HandleUtility.GetHandleSize (point);
		if (index == 0) {
			size *= 2f;
		}
		Handles.color = Color.cyan;
		if (Handles.Button (point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap)) {
			selectedIndex = index;
			Repaint ();
		}
		if (selectedIndex == index) {
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle (point, handleRotation);
			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (mesh, "Move Point");
				EditorUtility.SetDirty (mesh);
				mesh.SetPoint (index, handleTransform.InverseTransformPoint (point));
			}
		}
		return point;
	}

}