using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThirdPersonVariables
{
	[Header("3rd Person Properties")]
	[SerializeField]
	private LayerMask walls;
	
	[SerializeField, Header("Camera Variables")]
	private float upperCameraBound;
	[SerializeField]
	private float lowerCameraBound, cameraAdjustSpeed;

	public float cameraDistance;
	
	[SerializeField, Header("Movement Variables")]
	private float walkSpeedMod = 1f;

	[SerializeField]
	private KeyCode[] inputs;

	public void UpdateInputsAndEnums(KeyCodenames[] lists)
	{
		inputs = new KeyCode[lists.Length];
		for (int i = 0; i < lists.Length; i++)
		{
			inputs[i] = lists[i].Key;
		}
	}

	public Vector3 HorizontalBasedForward(Vector3 current)
	{
		return new Vector3(current.x, 0, current.z).normalized;
	}

	public LayerMask GetWalls
	{
		get { return walls; }
	}

	public float GetUpperBound
	{
		get { return upperCameraBound; }
	}

	public float GetLowerBound
	{
		get { return lowerCameraBound; }
	}

	public float AdjustSpeed
	{
		get { return cameraAdjustSpeed; }
	}

	public float GetWalkSpeedMod
	{
		get { return walkSpeedMod; }
	}

	public KeyCode[] Inputs
	{
		get { return inputs; }
	}
}
