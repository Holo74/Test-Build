using UnityEngine;
using System.Collections;

#pragma warning disable 0618

public class ThirdPersonCameraControl : MonoBehaviour {

    [Header("Camera Target")]
	[Space]
	public Transform cameraTarget;

    [Header("Sensitivity")]
	[Range(0.1f, 10f)]
	[Space]
	public float mouseXSpeedMod = 1;

    [Space]

    [Header("Camera Settings")]
	[Space]
	public float maxViewDistance = 25;
	public float minViewDistance = 3;
	public int zoomRate = 30;
	public int lerpRate = 10;

    public float cameraTargetHeight = 1.0f;

    private float distance = 3;
	private float desiredDistance;
	private float correctedDistance;
	private float currentDistance;

    private float x = 0.0f;
    private float y = 0.0f;

	//private ChangeStances changeStances;

	bool isCorrected = false;

	[Header("Input Settings")]
	[SerializeField]
	string userLookInputX = "Look X";

	[SerializeField]
	string userLookInputY = "Look Y";



	void Start () 
	{
		Vector3 angles = transform.eulerAngles; 
		x = angles.x;
		y = angles.y;

		currentDistance = distance;
		desiredDistance = distance;
		correctedDistance = distance;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;


	}
	
	//updates after update function, since our camera controls are not as important as our movement, we want our movement to occur first
	void LateUpdate ()
		{

		x += Input.GetAxis(userLookInputX) * mouseXSpeedMod;
		y -= Input.GetAxis(userLookInputY) * mouseXSpeedMod;

	

		y = ClampAngle (y, -50, 80);
		
		Quaternion rotation = Quaternion.Euler (y, x, 0);

		desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
		desiredDistance = Mathf.Clamp (desiredDistance, minViewDistance, maxViewDistance);
		correctedDistance = desiredDistance;


		Vector3 position = cameraTarget.position - (rotation * Vector3.forward * desiredDistance);
		
		RaycastHit collisionHit;
	    Vector3 cameraTargetPosition = new Vector3(cameraTarget.position.x, cameraTarget.position.y + cameraTargetHeight, cameraTarget.position.z);

		isCorrected = false;

		if(Physics.Linecast(cameraTargetPosition, position, out collisionHit))
		{
			if (collisionHit.transform.tag == "Ground" || collisionHit.transform.tag == "Building")
			{
				position = collisionHit.point;
				correctedDistance = Vector3.Distance(cameraTargetPosition, position);
				isCorrected = true;
			}
		}
	
		if(!isCorrected || correctedDistance > currentDistance)
		{
			currentDistance = Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomRate);
		}
		else
		{
			currentDistance = correctedDistance;
		}

		position = cameraTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -cameraTargetHeight, 0));

		//transform.rotation = rotation;
		//transform.position = position;

		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 55f * Time.deltaTime);
		transform.position = Vector3.Lerp(transform.position, position, 55f * Time.deltaTime);

		//?:
		//condition ? first_expression : second_expression;

	} 	

	private static float ClampAngle (float angle, float min, float max)
		{
			if (angle < -360) 
			{	
				angle -= 360;
			}
			if (angle > 360)
			{
				angle += 360;
			}
			return Mathf.Clamp(angle, min, max);
		}

}
