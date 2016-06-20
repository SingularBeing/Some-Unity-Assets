using UnityEngine;
using System.Collections;

//using UnityEditor;
//using System;
//using System.IO;
//using System.Xml;
//using System.Collections.Generic;

/*
	[Script Info]

*/
using System;


public class LineMeshWalk : MonoBehaviour
{

	public enum LineWalkerMode
	{
		Once,
		Loop,
		PingPong
	}

	public LineMesh mesh;

	public float duration;
	//public bool lookForward;
	public float progress;
	public LineWalkerMode mode;

	private bool goingForward = true;

	private int index = 0;

	private void Update ()
	{
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == LineWalkerMode.Once) {
					progress = 1f;
				} else if (mode == LineWalkerMode.Loop) {
					progress -= 1f;
				} else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		} else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		if (progress > 0.05f) {
			progress = 0.05f;
		}

		Vector3 position = Vector3.Lerp (transform.position, mesh.GetPoint (index), Mathf.Clamp01 (progress));
		if (Vector3.Distance (transform.position, mesh.GetPoint (index)) < 0.2f) {
			transform.position = mesh.GetPoint (index);
			index++;
			if (index > mesh.ControlPointCount - 1) {
				if (mode == LineWalkerMode.PingPong) {
					mesh.ReversePoints ();
					index = 0;
				} else if (mode == LineWalkerMode.Once) {
					index = mesh.ControlPointCount - 1;
				} else {
					index = 0;
				}
			}
		}
		transform.localPosition = position;
		//if (lookForward) {
		//	transform.LookAt (position + spline.GetDirection (progress));
		//}
	}

}