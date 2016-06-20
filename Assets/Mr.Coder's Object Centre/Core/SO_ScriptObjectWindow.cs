using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

///<summary>
///#DESCRIPTION OF CLASS#
///</summary>
public class SO_ScriptObjectWindow : EditorWindow 
{
    #region Variables
    //The current name of the object
    public static string objectName;
    //The current type of the object
    public static Object objectType;
	#endregion

	#region Methods
    [MenuItem("Window/Mr.Coder's Object Centre")]
    public static void Init()
    {
        //Create the window
        SO_ScriptObjectWindow window = (SO_ScriptObjectWindow)GetWindow(typeof(SO_ScriptObjectWindow), true, "Mr.Coder's Object Centre");
        //Show the window
        window.Show();
        //Focus the window
        window.Focus();
    }

    void OnGUI()
    {
        //Display a title
        GUILayout.Label(new GUIContent("General Settings", "No really, you will use these most often!"), EditorStyles.boldLabel);
        //The name of the object
        objectName = EditorGUILayout.TextField(new GUIContent("Name of object", "Think hard... think long... ah, hurry up!"), objectName);
        //The object to create a new ScriptableObject from
        objectType = (Object)EditorGUILayout.ObjectField(new GUIContent("Type of object", "What is the object we are referencing from?"), objectType, typeof(Object), false);

        EditorGUILayout.Space();

        //Display a create button
        if (GUILayout.Button("Create"))
        {
            string path = EditorUtility.SaveFilePanel("Create New ScriptableObject", "Assets/", objectName, "asset");

            if(path.Length != 0)
            {
                path = FileUtil.GetProjectRelativePath(path);

                objectType = CreateInstance(objectType.name);
                AssetDatabase.CreateAsset(objectType, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
	#endregion
}
