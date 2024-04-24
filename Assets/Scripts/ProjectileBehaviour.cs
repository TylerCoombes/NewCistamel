using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed;
    public PlayerController playerController;
    public GameObject gun;

    public LayerMask BulletCollide;

    private Rigidbody rb;

    public Vector3 bulletDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); //Gets the script off the player
        gun = GameObject.FindGameObjectWithTag("Gun"); //gets the object with tag gun
        rb.velocity = transform.right * Speed;
        bulletDir = rb.velocity;

        StartCoroutine(DestroyBullet());

        playerController.projectileBehaviour = this.gameObject.GetComponent<ProjectileBehaviour>();
        playerController.knockBackDirection = bulletDir - bulletDir * 2;
       // playerController.rb.velocity = playerController.knockBackDirection * playerController.thrustForce;
       // playerController.momentum = playerController.knockBackDirection.x * playerController.thrustForce;

        playerController.rb.AddForce(playerController.knockBackDirection * playerController.thrustForce, ForceMode.VelocityChange);

        print(playerController.knockBackDirection * playerController.thrustForce);
    }

    public void FixedUpdate()
    {
        //if(playerController.shot) playerController.rb.velocity = playerController.knockBackDirection * playerController.thrustForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(BulletCollide != 8)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator DestroyBullet()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(2);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);

        Destroy(gameObject);
    }
}
