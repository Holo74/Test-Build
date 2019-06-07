using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPMovement : MonoBehaviour {

	public Animator anim;
	public BoxCollider hitBox;

	[Space]
	public float playerSpeed = 4f;

	[Space]

	[Header("Input Settings")]
	public float inputX;
	public float inputY;
	public string inputXString = "Horizontal";
	public string inputZString = "Vertical";


	public float desiredRotationSpeed = 0.1f;
	public float allowPlayerRotation = 0f;
	public float speed= 0f;
	public float distanceRaycast = 0.1f;
	public float invincibleTime = 0.6f;
	public float jumpStrength = 6f;

	private Vector3 desiredMoveDirection;
	private Vector3 moveVector;
	private Camera cam;
	private CharacterController controller;
	public float verticalVel;

	[Space]
	Coroutine currentFreeRollCoroutine;
	Coroutine currentJumpCoroutine;

	[Space]

	public bool blockRotationPlayer;
	public bool isGrounded;
	public bool isMoving = false;
	public bool isSprinting = false;
	public bool isRolling = false;
	// Use this for initialization
	void Start ()
	{
		cam = Camera.main;
		//anim = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update ()
	{
		InputMagnitude();
		RollFreely();

		if(isGrounded == true)
		{
			if(Input.GetButtonDown("Jump"))
			{
				if(currentJumpCoroutine == null)
				{
					StartCoroutine(RunJump());
				}

				JumpStarted();
			}
		}

		if (Input.GetButtonUp("Jump"))
		{
			JumpEnded();
		}

		moveVector = new Vector3(0, verticalVel, 0);
		controller.Move(moveVector);
		//End of Grounded Check

		//Check if your moving
		if(Input.GetAxis(inputXString) == 0 && Input.GetAxis(inputZString) == 0)
		{
			isMoving = false;
		}
		else
		{
			isMoving = true;
		}
		//

		//Character Grounded Check
		RaycastHit raycastHit;
		if (Physics.Raycast(transform.position, -Vector3.up, out raycastHit, distanceRaycast))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}

		if (isGrounded == true)
		{
			verticalVel = 0;
		}
		else
		{
			verticalVel -= 0.5f * Time.deltaTime;
		}

	}

	void PlayerMoveAndRotation()
	{
		inputX = Input.GetAxis(inputXString);
		inputY = Input.GetAxis(inputZString);

		Camera camera = Camera.main;
		Vector3 forward = cam.transform.forward;
		Vector3 right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize();
		right.Normalize();

		desiredMoveDirection = forward * inputY + right * inputX;
		
		if (blockRotationPlayer == false)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
		}

		controller.Move(desiredMoveDirection * playerSpeed * Time.deltaTime);
	}

	void InputMagnitude()
	{
		//Calculate Input Vectors
		inputX = Input.GetAxis(inputXString);
		inputY = Input.GetAxis(inputZString);

		//anim.SetFloat("InputZ", inputZ, 0.0f, Time.deltaTime * 2f); Make sure to remove the comments with animations
		//anim.SetFloat("InputX", inputX, 0.0f, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		speed = new Vector2(inputX, inputY).sqrMagnitude;
		Vector3 direction = new Vector3(inputX, 0, inputY);

		//Physically move the player
		if(speed > allowPlayerRotation)
		{
			//anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
			PlayerMoveAndRotation();
		}
		else if(speed < allowPlayerRotation)
		{
			//anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
		}

	}


	//Rolling

	private IEnumerator FreeRoll()
	{
		isRolling = true;
		hitBox.enabled = false;
		yield return new WaitForSeconds(invincibleTime);
		anim.SetBool("isRolling", false);
		hitBox.enabled = true;
		isRolling = false;
	}

	void RollFreely()
	{
		if (isRolling == false)
		{

			if (Input.GetButtonDown("Roll"))
			{
				anim.SetBool("isRolling", true);

				if (currentFreeRollCoroutine != null)
				{
					StopCoroutine(currentFreeRollCoroutine);
				}

				currentFreeRollCoroutine = StartCoroutine(FreeRoll());
			}
		}


		if (Input.GetButtonUp("Roll"))
		{
			anim.SetBool("isRolling", false);
		}
	}

	void JumpStarted()
	{
		verticalVel = jumpStrength;
		anim.SetBool("RunJump", true);
	}

	void JumpEnded()
	{
		anim.SetBool("RunJump", false);
	}

	private IEnumerator RunJump()
	{
		yield return new WaitForSeconds(0.5f);
		anim.SetBool("RunJump", false);
		currentJumpCoroutine = null;
	}

}
