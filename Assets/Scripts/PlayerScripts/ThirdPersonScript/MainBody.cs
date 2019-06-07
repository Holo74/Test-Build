using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBody : MonoBehaviour
{
	public GameObject Camera
	{
		get; private set;
	}
	public GameObject Body
	{
		get { return transform.gameObject; }
	}
	public ThirdPersonVariables Variables
	{
		get
		{
			return variables;
		}
		set
		{
			variables = value;
		}
	}
	[SerializeField]
	private ThirdPersonVariables variables;

	public delegate void Updates();
	public event Updates update = () => { };

	public MainBody GetBody(Updates nUpdate)
	{
		update += nUpdate;
		return this;
	}

	public MainBody GetBody()
	{
		return this;
	}

	public ImproveRotate rotation
	{
		get; private set;
	}
	public InputHandler inputs
	{
		get; private set;
	}
	public ImprovedMovement movement
	{
		get; private set;
	}
	public CharacterController controller
	{
		get; private set;
	}

	public KeyCodenames[] keyCodes = new KeyCodenames[0];

	private void Awake()
	{
		Camera = FindObjectOfType<Camera>().gameObject;
		controller = GetComponent<CharacterController>();
	}

	void Start ()
	{
		rotation = new ImproveRotate(this);
		inputs = new InputHandler(this);
		movement = new ImprovedMovement(this);
		Camera.transform.LookAt(transform);
	}
	
	void Update ()
	{
		update();
	}
}
