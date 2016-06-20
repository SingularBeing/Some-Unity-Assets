using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CYRO;

namespace CYRO
{

	[System.Serializable]
	public class SCollisions
	{
		public enum ColliderType
		{
			PolygonCollider,
			BoxCollider,
			CircleCollider
		}

		public ColliderType colliderType;

		public List<Vector2> vertexPoints = new List<Vector2> ();

		public void GenerateCollisions ()
		{
		
		}

		public void AddCollisionNode ()
		{
		
		}

		public void Triangulate ()
		{
		
		}

	}

}