using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CYRO;

namespace CYRO
{
	
	public class SRoom : ScriptableObject
	{

		/*
		 * Backgrounds
		 */
		public bool drawBackgroundColor;
		public Color backgroundColor;
		public List<BackgroundNumber> backgroundNumbers = new List<BackgroundNumber> ();

		/*
		 * Settings
		 */
		public string roomName;
		public int roomWidth, roomHeight;
		public int roomSpeed;
		public int tileWidth, tileHeight;

		/*
		 * Physics
		 */
		public bool roomIsPhysicsWorld;
		public float xGravity, yGravity;
		//public float pixelsToMeters;

		/*
		 * Objects
		 */
		//this will have a popup box with all available
		//assets as objects
		public byte[] currentImageForObject;
		public int positionX, positionY;
		public float rotationAmount;
		public float scaleX, scaleY;
		public int alpha;
		public bool flipX, flipY;
		public Color blendingColor;
		//public SObject objectToAdd;
		public bool deleteUnderlying;

		/*
		 * Views
		 */
		public bool useViews;
		//public List<ViewNumber> views;
		public bool VvisibleWhenRoomStarts;
		public int viewInRoomX, viewInRoomY;
		public int viewInRoomWidth, viewInRoomHeight;
		public bool followingObject;
		public int horizontalBorder, verticalBorder;
		public int horizontalScrollspeed, verticalScrollspeed;

		/*
		 * Tiles
		 */
		//public SBackground currentTile;
		//public Dictionary<Vector2, SBackground> tiles;
		public bool deleteUnderlyingTile;
		public int currentTileLayer;
		public bool hideOtherLayers;

	}

}