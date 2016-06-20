using UnityEngine;
using System.Collections;
using CYRO;
using System.Collections.Generic;

namespace CYRO
{

	public class SceneObj : MonoBehaviour
	{
		public List<Vector3> points = new List<Vector3> ();

		public void CreatePoint (Vector3 pos)
		{
			points.Add (pos);
		}
	}

}