using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinamaticController : MonoBehaviour
{
    public Animator cinamaticAnimator;
    public PlayerController playerController;
    public GameObject Player;   

    public int blinkCounter;

    // Start is called before the first frame update
    void Start()
    {
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.isGrounded == true && blinkCounter == 0)
        {
            cinamaticAnimator.SetTrigger("isBlinking");
            Debug.Log("AnimationPlayed");
            blinkCounter++;
        }
    }
}
