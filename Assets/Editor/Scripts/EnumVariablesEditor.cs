using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnumVariables))]
public class EnumVariablesEditor : Editor
{
	EnumVariables variables;
	string filePath = "Assets/Scripts/Enums/";
	string fileName = "TestMyEnum";

	private void OnEnable()
	{
		variables = (EnumVariables)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		filePath = EditorGUILayout.TextField("Path", filePath);
		fileName = EditorGUILayout.TextField("Name", fileName);
		if (GUILayout.Button("Save"))
		{
			EditorFunctions.WriteToEnum(filePath, fileName, variables.variables);
		}
	}
}
