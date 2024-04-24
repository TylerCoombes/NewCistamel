 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
	public GameObject destroyed;

	// Start is called before the first frame update
	private void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			if (Vector3.Magnitude(collision.gameObject.GetComponent<Rigidbody>().velocity) > 14.8)
			{
				//Instantiate(destroyed, transform.position, transform.rotation);
				Destroy();
			}
		}

		if(collision.gameObject.tag == "Bullet")
        {
			//Instantiate(destroyed, transform.position, transform.rotation);
			Destroy();
		}
	}

    public void Destroy()
    {
		Instantiate(destroyed, transform.position, transform.rotation);
		Destroy(gameObject);
    }
}
