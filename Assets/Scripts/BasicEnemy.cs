using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private bool isPlayerClose;
    private PlayerController playerController;
    private GameObject player;
    public int damageAmount;
    public int bulletDamage;

    public SpriteRenderer spriteRenderer;
    Color origionalColor;
    public float flashTime = 0.3f;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    public EnemyTakeDamage enemyTakeDamage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        healthBar = this.gameObject.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        origionalColor = spriteRenderer.color;
        damageAmount = 30;
        bulletDamage = 20;

        enemyTakeDamage = GetComponent<EnemyTakeDamage>();
    }

    void Update()
    {
        // If the player is close, carry out attack function.
        if (isPlayerClose == true)
        {
            AttackPlayer();
        }

        if(enemyTakeDamage.currentHealth <= 0)
        {
            Die();
        }
    }

    // Attack the player and damage health if they are nearby.
    void AttackPlayer()
    {
        playerController.currentHealth = playerController.currentHealth - damageAmount;
        Debug.Log("Player health is " + playerController.currentHealth + ".");
        isPlayerClose = false; //set the variable to false at the end so health isn't continuously depleted.
    }

    //Check if the player is nearby.
    /*private void ontriggerenter(collider other)
    {
        if (other.gameobject.tag == "player")
        {
            isplayerclose = true;
            debug.log("player is nearby.");
        }
    }*/

    public void Die()
    {
        Destroy(gameObject);
    }


    /// <summary>
    /// Commented this all out because i made another script called EnemyTakeDamage and attached this to all types of enemy along with
    /// their stand alone script so we can call functions to do damage to them
    /// ive just copy and pasted this code below onto that EnemyTakeDamage script
    /// </summary>

    public void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Bullet Collided");
            TakeDamage(bulletDamage);
            FlashRed();
        }*/

        if (collision.gameObject.tag == "Player")
        {
            isPlayerClose = true;
            Debug.Log("Player is nearby.");
        }
    }

    /*public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        spriteRenderer.color = Color.red;
        spriteRenderer.color = Color.white;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);

        //Die Animation

        //Die
    }

    public void FlashRed()
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        spriteRenderer.color = origionalColor;
    }*/
}
