using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructibleDecay : MonoBehaviour
{
    private float decaySpeed = 0.5f;
    private bool decay;

    public Material materialT;

    void Start()
    {
        
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(waitTime());
    }

    IEnumerator waitTime()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(15);

        decay = true;
        this.GetComponent<MeshRenderer>().material = materialT;
    }

	private void Update()
	{
		if (decay)
		{
            Color color = gameObject.GetComponent<MeshRenderer>().material.color;
            float decayAmount = color.a - (decaySpeed * Time.deltaTime);

            this.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, decayAmount);

            if (this.GetComponent<MeshRenderer>().material.color.a <= 0)
			{
                Destroy(gameObject);
			}
        }
	}
}
