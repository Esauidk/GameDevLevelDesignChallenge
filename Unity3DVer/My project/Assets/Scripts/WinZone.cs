using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
	public bool hideOnAwake;

	private void Awake()
	{
		if (hideOnAwake)
		{
			GetComponent<MeshRenderer>().enabled = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			LevelLoaderScript.instance.LoadNextScene();
		}
	}
}
