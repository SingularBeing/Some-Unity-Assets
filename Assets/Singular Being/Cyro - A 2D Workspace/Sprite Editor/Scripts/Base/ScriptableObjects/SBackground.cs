using UnityEngine;
using System.Collections;
using UnityEditor;
using CYRO;

namespace CYRO
{

	public class SBackground : ScriptableObject
	{
		//the image
		[HideInInspector]
		public byte[] bytes;
		public bool useForTiles;
		public int textureWidth, textureHeight;
	}

}