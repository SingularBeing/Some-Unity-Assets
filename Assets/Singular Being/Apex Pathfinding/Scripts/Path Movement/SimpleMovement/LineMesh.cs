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


public class LineMesh : MonoBehaviour
{

	[SerializeField]
	private Vector3[] points;

	public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 GetPoint (int index)
	{
		if (index > points.Length - 1) {
			Debug.Log ("Index:" + index);
			return new Vector3 (0, 0, 0);
		}
		return points [index];
	}

	public void SetPoint (int index, Vector3 point)
	{
		points [index] = point;
	}

	public void ReversePoints ()
	{
		Array.Reverse (points);
	}

	public void Reset ()
	{
		points = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0)
		};
	}

	public void AddLine ()
	{
		//add a new line to the mix
		Array.Resize (ref points, points.Length + 2);
		points [points.Length - 2] = new Vector3 (points [points.Length - 3].x + 1, points [points.Length - 3].y, 0);
		points [points.Length - 1] = new Vector3 (points [points.Length - 2].x + 1, points [points.Length - 2].y, 0);
	}

}