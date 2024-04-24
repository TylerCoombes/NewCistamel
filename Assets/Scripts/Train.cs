using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float speed;
    public Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "TrainReset")
        {
            Debug.Log("HitReset");
            gameObject.transform.position = originalPos;
        }
    }
}
