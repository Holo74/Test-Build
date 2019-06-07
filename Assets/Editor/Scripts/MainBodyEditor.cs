using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(MainBody))]
public class MainBodyEditor : Editor
{
	private MainBody body;

	private void OnEnable()
	{
		body = (MainBody)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Apply to Enums and Variables"))
		{
			string[] names = new string[body.keyCodes.Length];
			body.Variables.UpdateInputsAndEnums(body.keyCodes);
			EditorUtility.SetDirty(body);
			EditorUtility.SetDirty(body.gameObject);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			for (int i = 0; i < names.Length; i++)
			{
				names[i] = body.keyCodes[i].Name;
			}
			EditorFunctions.WriteToEnum("Assets/Scripts/Enums/", "InputVariables", names);
		}
	}
}
