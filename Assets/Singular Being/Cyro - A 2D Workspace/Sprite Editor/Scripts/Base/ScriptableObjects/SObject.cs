using UnityEngine;
using System.Collections;
using CYRO;

namespace CYRO
{

	public class SObject : ScriptableObject
	{
		//Physics
		public float phys_mass = 1;
		public float phys_linearDrag;
		public float phys_angularDrag = 0.05f;
		public float phys_gravityScale = 1;

		public bool phys_kinematic = false;
		public bool phys_unmoveable;

		public RigidbodySleepMode2D phys_sleep = RigidbodySleepMode2D.StartAwake;
		public RigidbodyInterpolation2D phys_interpolation = RigidbodyInterpolation2D.None;
		public RigidbodyConstraints2D phys_constraints = RigidbodyConstraints2D.None;

		//Other
		public SSprite sprite;

		public int layerDepth;
		public bool destroyedOnLoad;

		public string assetLocation;

	}

}