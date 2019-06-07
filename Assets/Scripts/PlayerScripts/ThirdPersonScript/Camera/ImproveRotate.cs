using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImproveRotate : IUpdatable
{
	private MainBody controller;
	private RaycastHit wallLocation;
	private float timeLerp = 1, distanceFromTarget;

	private GameObject camera
	{
		get
		{
			return controller.Camera;
		}
	}
	private GameObject body
	{
		get
		{
			return controller.Body;
		}
	}
	private Vector3 Direction
	{
		get
		{
			return (camera.transform.position - body.transform.position).normalized;
		}
	}
	private Vector3 ModifiedCameraLocation
	{
		get
		{
			return Direction * distanceFromTarget + body.transform.position;
		}
	}
	private Vector3 SupposedLocation
	{
		get
		{
			return Direction * controller.Variables.cameraDistance + body.transform.position;
		}
	}
	private Vector3 BodyAdjustedForward
	{
		get
		{
			return new Vector3(camera.transform.forward.x, body.transform.forward.y, camera.transform.forward.z);
		}
	}
	private bool CameraAbove
	{
		get
		{
			return camera.transform.position.y > body.transform.position.y;
		}
	}
	public float CameraAngle
	{
		get
		{
			return Vector3.Angle(BodyAdjustedForward, camera.transform.forward) * (CameraAbove ? 1 : -1);
		}
	}
	
	public ImproveRotate(MainBody body)
	{
		controller = body.GetBody(IUpdate);
	}

	public void IAwake()
	{
		
	}

	public void IStart()
	{

	}

	public void IUpdate()
	{
		WallDetection();
		RotatingHorizontally();
		RotateVertically();
		camera.transform.LookAt(body.transform);
	}

	private float DistanceValue(float values)
	{
		if(values > 1)
		{
			return 1;
		}
		return values * .9f;
	}

	private void WallDetection()
	{
		if (Physics.Raycast(body.transform.position, Direction, out wallLocation, 5, controller.Variables.GetWalls))
		{
			camera.transform.position = ((wallLocation.distance - DistanceValue(wallLocation.distance)) * Direction) + body.transform.position;
			distanceFromTarget = wallLocation.distance - 1f;
			timeLerp = 0;
		}
		else
		{
			if (timeLerp <= 1.1f)
			{
				camera.transform.position = Vector3.Lerp(ModifiedCameraLocation, SupposedLocation, timeLerp);
			}
			timeLerp = Mathf.Clamp(timeLerp + Time.fixedDeltaTime * controller.Variables.AdjustSpeed, 0f, 1.2f);
		}
	}

	private void RotatingHorizontally()
	{
		if(controller.inputs.MouseX != 0f)
			camera.transform.RotateAround(body.transform.position, Vector3.up, controller.inputs.MouseX);
	}

	private void RotateVertically()
	{
		if (controller.inputs.MouseY == 0f)
			return;

		float rotate = controller.inputs.MouseY;

		if (CameraAbove)
		{
			if (rotate < 0)
				if (CameraAngle - rotate > controller.Variables.GetUpperBound)
					rotate = 0;
		}
		else
		{
			if (rotate > 0)
				if (CameraAngle - rotate < controller.Variables.GetLowerBound)
					rotate = 0;
		}

		camera.transform.RotateAround(body.transform.position, camera.transform.right, -rotate);
	}
}
