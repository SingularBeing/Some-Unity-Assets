using UnityEngine;
using System.Collections;
using CYRO;
using System.Collections.Generic;

namespace CYRO
{
	[System.Serializable]
	public class BackgroundTilemap
	{

		public int tileWidth, tileHeight;
		public int horizontalOffset, verticalOffset;
		public int horizontalStep, verticalStep;
		public List<byte[]> textureBytes = new List<byte[]> ();

	}

}