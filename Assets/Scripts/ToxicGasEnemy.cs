using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

public class ToxicGasEnemy : MonoBehaviour
{
    private bool isPlayerClose;
    private PlayerController playerController;
    private GameObject player;
    public float damageAmount = 7f; // Damage amount to be decided.

    public float distanceFromPlayer = 20f; //When player is within this radius they will start to lose health.

    public SpriteRenderer spriteRenderer;

    public HealthBar healthBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        healthBar = this.gameObject.GetComponentInChildren<HealthBar>();
    }

    void Update()
    {
        // If the player is close, carry out attack function.
        if (isPlayerClose == true)
        {
            AttackPlayer();
        }
        DetectPlayer();
    }

    // Check to see if player is within distance to suffer effects of toxic gas.
    // Radius can be changed.
    public void DetectPlayer()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < distanceFromPlayer)
        {
            UnityEngine.Debug.Log("Player is nearby.");
            isPlayerClose = true;
        }
        else
        {
            isPlayerClose = false;
        }
    }

    // Attack the player and damage health if they are nearby.
    void AttackPlayer()
    {
        playerController.currentHealth = playerController.currentHealth - damageAmount * Time.deltaTime * 1;
        //UnityEngine.Debug.Log("Player health is " + playerController.currentHealth + ".");

    }
}
