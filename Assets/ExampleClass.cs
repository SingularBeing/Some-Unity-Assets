using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
	public int toolbarInt = 0;
	public string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };

	void OnGUI ()
	{
		toolbarInt = GUI.Toolbar (new Rect (25, 25, 250, 30), toolbarInt, toolbarStrings);
	}
}