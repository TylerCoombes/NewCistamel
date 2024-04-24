using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public GameObject Player;
    public PlayerController playerController;
    public int increaseAmount;

    private void Start()
    { 
        //playerController = Player.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(playerController.currentHealth < 100)
            {
                playerController.currentHealth += increaseAmount;
                Destroy(gameObject);
            }
        }
    }
}
