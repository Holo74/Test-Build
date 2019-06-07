using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameObject playerPrefs;
	private GameObject player;
	private Transform spawn;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		if(player == null)
		{
			player = Instantiate(playerPrefs);
			player.transform.position = spawn.position;
		}
	}
}
