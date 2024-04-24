using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_trigger : MonoBehaviour
{

	public GameObject platform;


	private void OnTriggerEnter(Collider other)
	{	
		if (!platform.GetComponent<MovingPlatform>().enabled)
		{
			if (other.gameObject.tag == "Player")
			{
				platform.GetComponent<MovingPlatform>().enabled = true;
			}
		}

	}

}
