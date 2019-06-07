using UnityEngine;

public class Rotate : MonoBehaviour
{
	public GameObject playerCamera;
	public Transform body;
	public LayerMask walls;

	private void Start()
	{
		Vector3 temp;
		temp = body.position;
		temp.x += 5;
		temp.y += 5;
		playerCamera.transform.position = temp;
		playerCamera.transform.LookAt(body);
		playerCamera.transform.SetParent(body);
	}

	private void Update()
	{
		
		DetectPosition();
		
		
	}

	private Vector3 modified;
	private float time = 1, distance;
	private bool cameraAbove;
	public float adjustSpeed, lowerBound, upperBound;
	private float currentYRotation;
	public void DetectPosition()
	{
		float mouseX = Input.GetAxis("Mouse X"), mouseY = Input.GetAxis("Mouse Y");
		RaycastHit wall;
		Vector3 direction = (playerCamera.transform.position - body.position).normalized;
		cameraAbove = playerCamera.transform.position.y > body.position.y ? true : false;
		Vector3 supposedLocation = direction * 5 + body.position;
		if (Physics.Raycast(body.position, direction, out wall, 5, walls))
		{
			playerCamera.transform.position = ((wall.distance - 1f) * direction) - body.position;
			distance = Vector3.Distance(playerCamera.transform.position, body.position);
			
			time = 0;
			
		}
		else
		{
			modified = direction * distance + body.position;
			if(time != 1f)
			{
				playerCamera.transform.position = Vector3.Lerp(modified, supposedLocation, time);
			}
			time = Mathf.Clamp(time + Time.deltaTime * adjustSpeed, 0, 1f);
		}
		if (mouseX != 0.0f)
		{
			playerCamera.transform.RotateAround(body.position, Vector3.up, mouseX);
		}
		if (mouseY != 0.0f)
		{
			Vector3 temp = new Vector3(playerCamera.transform.forward.x, body.forward.y, playerCamera.transform.forward.z);
			float hold = Vector3.Angle(temp, playerCamera.transform.forward) * (cameraAbove ? 1 : -1);
			if (cameraAbove)
			{
				if(mouseY < 0)
				{
					if (hold - mouseY > upperBound)
					{
						mouseY = 0;
					}
				}
				
			}
			else
			{
				if(mouseY > 0)
				{
					if (hold - mouseY < lowerBound)
					{
						mouseY = 0;
					}
				}
				
			}
			playerCamera.transform.RotateAround(body.position, playerCamera.transform.right, -mouseY);
			
		}
	}
}
