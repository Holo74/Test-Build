using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedMovement : IUpdatable
{
	private MainBody body;
	private ThirdPersonVariables variables
	{
		get
		{
			return body.Variables;
		}
	}
	private InputHandler input
	{
		get
		{
			return body.inputs;
		}
	}
	private Vector3 rotationVec = new Vector3();

	private Vector3 moveValue;
	public Vector3 MoveValue
	{
		get
		{
			return moveValue;
		}
		set
		{
			moveValue += value;
		}
	}

	private Vector3 beforeMove;

	public ImprovedMovement(MainBody _body)
	{
		body = _body.GetBody(IUpdate);
	}

	public void IAwake()
	{

	}

	public void IStart()
	{

	}

	public void IUpdate()
	{
		Moving(InputVariables.moveForward, body.Camera.transform.forward);
		Moving(InputVariables.moveLeft, -body.Camera.transform.right);
		Moving(InputVariables.moveBack, -body.Camera.transform.forward);
		Moving(InputVariables.moveRight, body.Camera.transform.right);

		FinalizedMove();
	}

	private void FinalizedMove()
	{
		beforeMove = body.transform.position;
		body.controller.Move(MoveValue * Time.fixedDeltaTime * variables.GetWalkSpeedMod);
		beforeMove -= body.transform.position;
		rotationVec.Normalize();
		body.transform.LookAt(body.transform.position + rotationVec);
		body.Camera.transform.position -= beforeMove;
		moveValue = Vector3.zero;
	}

	private void Moving(InputVariables input, Vector3 direction)
	{
		if (this.input.Key(InputType.hold, input))
		{
			MoveValue = variables.HorizontalBasedForward(direction);
			rotationVec += variables.HorizontalBasedForward(direction) * .9f;
		}
	}
}
