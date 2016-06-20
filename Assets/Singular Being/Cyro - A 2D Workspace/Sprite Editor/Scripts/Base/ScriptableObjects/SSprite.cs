using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CYRO;

namespace CYRO
{

	public class SSprite : ScriptableObject
	{

		//location in assets
		public string assetLocation = "";
		//the sprite's collisions
		public SCollisions collisions = new SCollisions ();
		//do we use an external texture?
		public bool usesInternalTexture = true;
		//the texture's location
		public string textureLocation;
		//the texture's base color addition
		public Color textureColorAddition = new Color (1, 1, 1, 1);
		//misc
		public bool flipX, flipY;
		public string materialLocation;
		public int currentDepth;


		//origin point for the sprites
		//public Vector2 origin;
		//the width & height for this sprite
		//public int width, height;

		public SSprite ()
		{
			/*if (textures.Count == 0) {
				textures.Add (new PaintImage ());
				textures [textures.Count - 1].parentObject = this;
				textures [textures.Count - 1].Init (32, 32);
			}*/
		}

	}

}