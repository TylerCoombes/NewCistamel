using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lr;

    public LayerMask grappleMask;
    public float enemyPullSpeed;
    public float playerPullSpeed;
    public float grappleLength = 10;

    public int maxPoints = 30;

    public bool shot;
    public bool grappling;

    public List<Vector2> points = new List<Vector2>();
    public Vector2 hitPoint;

    public GameObject grappledObject;
    public Rigidbody grappledRb;
    public Grapple_Rotation grappleRotation;

    public GameObject Player;
    public Rigidbody playerRb;
    public PlayerController playerController;

    public GrappledObject grappledObjectScript;
    public float damping;


    // Start is called before the first frame update
    void Start()
    {
        grappling = false;
        playerRb = GetComponentInParent<Rigidbody>();
        playerController = Player.GetComponent<PlayerController>();
        lr.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && shot == false)
        {
            if (grappleRotation.hit.collider != null)
            {
                Debug.Log("collider hit");
                hitPoint = grappleRotation.transform.parent.position;
                if (grappleRotation.hit.transform.tag == "Grappleable")
                {
                    grappledObject = grappleRotation.hit.transform.parent.gameObject;
                    
                }
                else if (grappleRotation.hit.transform.tag == "Enemy")
                {
                    grappledObject = grappleRotation.hit.transform.gameObject;
                    
                }
                
                points.Add(hitPoint);
                //points.Add(grappledObject.transform.position);

                if (points.Count > maxPoints)
                {
                    points.RemoveAt(0);
                }
            }

            if (points.Count > 0)
            {

                if (grappledObject.transform.tag == "Enemy") //pulling enemy to player
                {
                    playerController.audioManager.PlayGrappleClip();


                    grappledRb = grappledObject.GetComponent<Rigidbody>();
                    
                    float distance = Vector3.Distance(transform.position, grappledObject.transform.position);
                    Vector3 pullDirection = (transform.position - grappledObject.transform.position);
                    grappledRb.velocity = (pullDirection * distance * enemyPullSpeed) / grappledRb.mass;

                    float dampingValue = 1 - (1 / (distance * damping + 1));
                    grappledRb.velocity *= dampingValue;


                    lr.positionCount = 0;
                    lr.positionCount = points.Count * 2;                   
                }
                else if (grappledObject.transform.tag == "Grappleable") //pulling player to object
                {
                    playerController.audioManager.PlayGrappleClip();

                    Vector2 grapplePull = new Vector2(grappledObject.transform.position.x - Player.transform.position.x, grappledObject.transform.position.y - Player.transform.position.y);
                    Debug.Log(grapplePull);
                    playerController.momentum = grapplePull.x * playerPullSpeed;
                    playerRb.velocity = grapplePull * playerPullSpeed;
                    shot = true;
                    grappling = true;
                    StartCoroutine(GrappleCoolDown());

                    lr.positionCount = 0;
                    lr.positionCount = points.Count * 2;
                }
            }
        }

        for (int n = 0; n < points.Count * 2; n += 2)
        {
            lr.SetPosition(n, transform.position);
           
            lr.SetPosition(n + 1, grappledObject.transform.position);
                 
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            Detatch();
        }
    }

    void Detatch()
    {
        lr.positionCount = 0;
        points.Clear();
    }

    public IEnumerator GrappleCoolDown()
    {
        yield return new WaitForSeconds(1);
        grappling = false;
        shot = false;
    }
}
