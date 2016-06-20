using UnityEngine;
using System.Collections;
using CYRO;
using System.Collections.Generic;
using System;

namespace CYRO
{

	public static class TextureEffects
	{

		public static Texture2D FlipTextureX (int width, int height, ref Texture2D texture)
		{
			Texture2D duplicate = new Texture2D (width, height);

			List<Color[]> horizontalList = new List<Color[]> ();

			for (int i = 0; i < height; i++) {
				List<Color> colors = new List<Color> ();
				for (int x = 0; x < width; x++) {
					colors.Add (texture.GetPixel (x, i));
				}
				horizontalList.Add (colors.ToArray ());
			}

			for (int i = 0; i < horizontalList.Count; i++) {
				Array.Reverse (horizontalList [i]);
				for (int x = 0; x < width; x++) {
					duplicate.SetPixel (x, i, horizontalList [i] [x]);
				}
			}

			duplicate.filterMode = FilterMode.Point;
			duplicate.alphaIsTransparency = true;
			//duplicate.Compress (false);
			duplicate.Apply ();

			return duplicate;
		}

		public static Texture2D FlipTextureY (int width, int height, ref Texture2D texture)
		{
			Texture2D duplicate = new Texture2D (width, height);

			List<Color[]> verticalList = new List<Color[]> ();

			for (int i = 0; i < width; i++) {
				List<Color> colors = new List<Color> ();
				for (int y = 0; y < height; y++) {
					colors.Add (texture.GetPixel (i, y));
				}
				verticalList.Add (colors.ToArray ());
			}

			for (int i = 0; i < verticalList.Count; i++) {
				Array.Reverse (verticalList [i]);
				for (int y = 0; y < width; y++) {
					duplicate.SetPixel (i, y, verticalList [i] [y]);
				}
			}

			duplicate.filterMode = FilterMode.Point;
			duplicate.alphaIsTransparency = true;
			//duplicate.Compress (false);
			duplicate.Apply ();

			return duplicate;
		}

	}

}