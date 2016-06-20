using UnityEngine;
using System.Collections;
//using UnityEditor;
//using System;
using System.IO;
//using System.Xml;
using System.Collections.Generic;

/*
	[Script Info]

*/
public class PersistComponent : MonoBehaviour {

    void OnDisable()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/PersistData/"))
            Directory.CreateDirectory(Application.persistentDataPath + "/PersistData/");

        SaveData();
    }

	void SaveData()
    {
        Component[] component = gameObject.GetComponents<Component>();
        foreach(Component c in component)
        {
            string currentJSON = JsonUtility.ToJson(c);
            //save the component's JSON string to a file
            StreamWriter write = new StreamWriter(Application.persistentDataPath + "/PersistData/" + gameObject.GetInstanceID() + "_" + c.name);
            write.Write(currentJSON);
            write.Close();
        }
    }

}