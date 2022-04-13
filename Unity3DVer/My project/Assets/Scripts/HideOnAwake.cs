using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HideOnAwake : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<MeshRenderer>().enabled = false;
	}

}
