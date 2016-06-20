using UnityEngine;
using System.Collections;
using CYRO;
using UnityEditor;

namespace CYRO
{

	public class InspectorTransformComponent
	{

		public static void Init (Rect rect, Transform transform, Color backColor)
		{
			EditorGUI.DrawRect (rect, backColor);

			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			Vector3 scale = transform.localScale;

			//display the name
			GUI.Label (new Rect (rect.x, rect.y, rect.width, 20), "Transform", EditorStyles.centeredGreyMiniLabel);
			//display the position
			GUI.Label (new Rect (rect.x + 5f, rect.y + 22.5f, 80f, 20f), "Position");
			position = EditorGUI.Vector3Field (new Rect (rect.x + 80f, rect.y + 25f, rect.width - 80f, 20f), "", position);
			//display the rotation
			GUI.Label (new Rect (rect.x + 5f, rect.y + 47.5f, 80f, 20f), "Rotation");
			rotation.eulerAngles = EditorGUI.Vector3Field (new Rect (rect.x + 80f, rect.y + 50f, rect.width - 80f, 20f), "", rotation.eulerAngles);
			//display the scale
			GUI.Label (new Rect (rect.x + 5f, rect.y + 72.5f, 80f, 20f), "Scale");
			scale = EditorGUI.Vector3Field (new Rect (rect.x + 80f, rect.y + 75f, rect.width - 80f, 20f), "", scale);
		}

	}

}