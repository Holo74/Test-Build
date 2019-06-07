using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : IUpdatable
{
	private MainBody controller;
	public const string mouseX = "Mouse X";
	public const string mouseY = "Mouse Y";
	public InputHandler(MainBody body)
	{
		controller = body.GetBody(IUpdate);
	}

	public float MouseX { get; private set; }
	public float MouseY { get; private set; }

	public void IAwake()
	{

	}

	public void IStart()
	{

	}

	public void IUpdate()
	{
		MouseX = Input.GetAxis(mouseX);
		MouseY = Input.GetAxis(mouseY);
	}

	public KeyCode GetInputs(InputVariables inputs)
	{
		return controller.Variables.Inputs[(int)inputs];
	}

	public bool Key(InputType type, InputVariables input)
	{
		switch (type)
		{
			case InputType.up:
				return Input.GetKeyUp(GetInputs(input));
			case InputType.down:
				return Input.GetKeyDown(GetInputs(input));
			case InputType.hold:
				return Input.GetKey(GetInputs(input));
			default:
				Debug.LogError("Unknown type");
				return false;
		}
	}
}

public enum InputType
{
	up,
	down,
	hold
}