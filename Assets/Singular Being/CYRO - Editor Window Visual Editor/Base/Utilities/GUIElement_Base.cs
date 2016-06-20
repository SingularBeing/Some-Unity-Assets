using UnityEngine;
using System.Collections;
using CYRO.EditorWindowVisualEditor;

namespace CYRO.EditorWindowVisualEditor
{
	/// <summary>
	/// Every GUIElement takes from this.
	/// </summary>
	public class GUIElement_Base
	{

		//the name of this element
		public string myName = "";
		//the position in the GUI
		public Rect myRect = new Rect ();
		//the identifier of this element
		public string myIndentifier = "";
		//the content of this element
		public GUIContent myContent = null;
		//the style of this element. IF null, the default style is accepted
		public GUIStyle myStyle = null;
		//the skin of this element. IF null, the default skin is accepted
		public GUISkin mySkin = null;
		//do we use a texture?
		public bool useTexture = false;
		//the current texture
		public Texture2D currentTexture;

		public override string ToString ()
		{
			return string.Format ("Rect: {0}, Identifier: {1}, Content: {2}, Style: {3}, Skin: {4}, Uses Texture: {5}", myRect, myIndentifier, myContent, "", "", useTexture);
		}

	}

	/// <summary>
	/// Every GUIElement that displays data.
	/// e.g. Labels, Boxes
	/// </summary>
	public class GUIElement_Display : GUIElement_Base
	{



	}

	/// <summary>
	/// Every GUIElement that accepts input AND contains an internal bool.
	/// e.g. Buttons
	/// </summary>
	public class GUIElement_Linkable : GUIElement_Base
	{



	}

	/// <summary>
	/// Every GUIElement that accepts text input
	/// e.g. TextFields, TextAreas
	/// </summary>
	public class GUIElement_TextInput : GUIElement_Base
	{



	}

	/// <summary>
	/// Every GUIElement that accepts a boolean value
	/// e.g. Toggles
	/// </summary>
	public class GUIElement_Boolean : GUIElement_Base
	{



	}

}