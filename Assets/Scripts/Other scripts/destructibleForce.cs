using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructibleForce : MonoBehaviour
{
    private GameObject player;
    private float timer = 20f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        addForceToChildObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
		{
            timer -= Time.deltaTime;
		}
		else
		{
            Destroy(gameObject);
		}

    }

    void addForceToChildObjects()
	{
        foreach (Transform child in transform)
		{
            float randomX = Random.Range(0, 2f);
            float randomY = Random.Range(0, 2f);
            float randomZ = Random.Range(-7f, 7f);
            Vector3 direction = player.GetComponent<Rigidbody>().velocity/7;

            child.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x * randomX, direction.y * randomY, randomZ), ForceMode.Impulse);

		}
    }
}
