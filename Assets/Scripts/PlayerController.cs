using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
	#region initialize
    [Header("Components")]
    public Rigidbody rb;
    public Animator animator;
    private Grapple grapple;

    [Header("Movement")]
    //movement 2.0
    public float moveInput;
    public float speed;
    public float friction;
    public float acceleration;
    public float momentum;
    public bool sliding;

    [Header("Jumping")]
    public bool isJumping;
    public float jumpForce;
    public float jumpTime;
    public float jumpHoldDelay;
    private float jumpHoldT;
    private bool jumpHeld;
    private float jumpTimeCounter;

    [Header("isGrounded check")]
    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    [Header("Misc.")]
    public bool lookingRight;
    public CinemachineVirtualCamera Cinecamera;
    public int CameraOffset = 5;
    public Transform Player;

    [Header("Shooting")]
    public gun_rotation gunRotation;
    public Transform launchOffset;
    public GameObject gun;
    public GameObject projectilePrefab;
    public ProjectileBehaviour projectileBehaviour;
    public Vector3 knockBackDirection;
    public float thrustForce;
    public bool isAiming;
    public bool isAimingRight;
    public bool shot;
    private bool shooting;
    public GameObject directionalArrow;

    [Header("MeleeAttack")]
    public Transform attackPoint;
    public Transform attackPointRight;
    public Transform attackPointLeft;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public LayerMask wallLayer;
    public int attackDamage = 40;
    public Collider[] hitEnemies;
    public Collider[] walls;
    public int punchNumber;
    private float timeSinceAttack;

    [Header("Health")]
    //Player Health Variables
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;
    public Scene_Reset reset;

    [Header("Respawn & CheckPoints")]
    //Respawn Point
    private Vector3 respawnPoint;
    public GameObject lastCheckPoint;
    public GameObject newCheckPoint;

    [Header("Trajectory Line")]
    public LineRenderer trajectoryLine; // Reference for the line renderer
    public Transform releasePos; // Position at which the bullet is fired
    [SerializeField]
    [Range(10, 100)]
    private int linePoints = 25; // Number of vertices on the line renderer
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float timeBetweenPoints = 0.1f; // Amount of space between each vertex on the line
    private float projectileForce = 60f; // force of the knockback 

    [Header("Audio Clips")]
    public AudioManager audioManager;
    public bool canPunchSound;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //gets rigid body from player
        grapple = GetComponentInChildren<Grapple>();
        lookingRight = true;
        shot = false;

        respawnPoint = transform.position; // Where the player starts in the game this is set as the first respawn point.
        currentHealth = maxHealth; //At start of play, set current health to the max value.
        healthBar.SetMaxHealth((int)maxHealth);
        attackPoint.position = attackPointRight.position;

    }

    // Update is called once per frame
    public void Update()
    {
        // Counts up the time since each attack
        timeSinceAttack += Time.deltaTime;

        // Runs the function for updating the player inputs
        UpdateInput();
        // Sphere check for ground bool
        isGrounded = Physics.CheckSphere(feetPos.position, checkRadius, whatIsGround);

        // Sets all the animation bools to work with the appropriate bools within the player controller
        animator.SetBool("is_grounded", isGrounded);
        animator.SetBool("is_aimingRight", isAimingRight);
        animator.SetBool("is_lookingRight", lookingRight);
        animator.SetFloat("momentum", momentum);

        //Sprite flip, can only change direction while grounded 
        if (isGrounded)
        {
            // If the player is moving right
            if (moveInput > 0)
            {
                UpdateLookingRight(true);
                attackPoint.position = attackPointRight.position;
            }
            // If the player is moving left
            else if (moveInput < 0) 
            {
                UpdateLookingRight(false);
                attackPoint.position = attackPointLeft.position;

            }
        }
        // Runs the jumping function
        UpdateJump();
        // Runs the gun function
        UpdateCannon();
        // Runs the slow motion functionality
        SlowMo();
        ResetSlowMo();
        // Function to check the players health
        CheckPlayerHealth();
        // Function that controls the shooting of the gun
        ShootShotGun();
        // Ensures the player health will not exceed the max value
        if (currentHealth > 100)
        {
            currentHealth = maxHealth;
        }

        // Checks if the left mouse button has been pressed if enough time has passed since the last attack
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.5f && !isAiming)
        {
            // Resets the time since the last attack
            timeSinceAttack = 0.0f;
            // Runs the function for the player to do damage
            Attack();
            Debug.Log(timeSinceAttack);
        }
        // Reset input by pressing "R"
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = respawnPoint;
        }
        // When the player is aiming the function for drawing the trajectory line will activate
        if (isAiming)
        {
            DrawProjection();
        }
        else
        {
            trajectoryLine.enabled = false;
        }

    }

    private void FixedUpdate()
    {
        // Updates the players physics based movement
        UpdateMovement();
        // Clamps the players max velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 22);
    }

    private void UpdateInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); //getting the horizontal move input

        //sprint input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded)
            {
                speed = 15;
                animator.speed = 1.2f;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10;
            animator.speed = 1;
        }

        //aim input
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;

            if (isAimingRight) //make sure to face the direction you are aiming
            {
                UpdateLookingRight(true);
            }
            else
            {
                UpdateLookingRight(false);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }     

    }

    private void UpdateMovement() //movement code
	{
        if ((moveInput < -0.1 || moveInput > 0.1) && !shooting && !grapple.grappling) //if the player is moving left or right and not aiming
        {
            animator.SetInteger("move_input", 1); //Animates the player movement

            if (isGrounded)
            {
                if (moveInput < -0.1) //If moving left
                {
                    if (momentum > -speed) 
                    {
                        momentum += moveInput * acceleration; //Begins accelerating the player to max speed
                    }
                }
                else if (moveInput > 0.1)
                {
                    if (momentum < speed)
                    {
                        momentum += moveInput * acceleration; //Begins accelerating the player to max speed
                    }
                }

                if (Mathf.Abs(momentum) > speed) //Smooth clamp
                {
                    momentum = momentum * 0.99f;
                }
            }
            else
            {
                if (moveInput < -0.1)
                {
                    if (momentum > -speed)
                    {
                        momentum += moveInput * acceleration / 4; //Less acceleration whilst airborn
                    }
                }
                else if (moveInput > 0.1)
                {
                    if (momentum < speed)
                    {
                        momentum += moveInput * acceleration / 4; //Less acceleration whilst airborn
                    }
                }
            }
        }
        else //if the player isnt inputing movement
        {
            if (isGrounded)
            {
                if (shooting)
                {
                    momentum = rb.velocity.x;
                }
                if (Mathf.Abs(momentum) > 0.1)
                {
                    momentum = momentum / friction; // apply cheap friction
                }
                else
                {
                    momentum = 0;
                }
            }
            if (!isGrounded)
            {
                momentum = rb.velocity.x;
            }
            
            animator.SetInteger("move_input", 0);
        }

        rb.velocity = new Vector2(momentum, rb.velocity.y); //add speed to the player's velocity
    }

    private void UpdateJump() //jump code
	{
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) //if the player is on the floor and the W key is pressed
        {
            isJumping = true; //set the isjumping bool to true
            animator.SetBool("is_jumping", true);
            jumpTimeCounter = jumpTime;
            jumpHoldT = Time.time;
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce); //adds force to the player to make them go up
            rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);


        }

        if (isJumping && Input.GetKey(KeyCode.Space)) //hold for longer jump
        {
            if (jumpTimeCounter > 0)
            {
                jumpTimeCounter -= Time.deltaTime;

                if (jumpHoldDelay < Time.time - jumpHoldT && !jumpHeld) //if jump is held longer than the minimum delay
                {
                    //rb.velocity = new Vector2(rb.velocity.x, jumpForce); //we continue to set the velocity up so the player is jumping
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
                    jumpHeld = true;
                }
            }
            else
            {
                isJumping = false;
                jumpHeld = false;
                animator.SetBool("is_jumping", false);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            jumpHeld = false;
            animator.SetBool("is_jumping", false);
        }
    }

    private void UpdateCannon()
	{
        if (isAiming)
        {
            animator.SetBool("is_aiming", true);
        }
        else
        {
            animator.SetBool("is_aiming", false);
        }
    }

    private void UpdateLookingRight(bool Boolean)
	{
        if (Boolean)
        {
            this.GetComponent<SpriteRenderer>().flipX = false; //dont change the transform rotation
            if (lookingRight != true)
            {
                lookingRight = true;
                Cinecamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x += CameraOffset;
            }   
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = true; //flip the player sprite to face the left
            if (lookingRight != false)
            {
                lookingRight = false;
                Cinecamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x -= CameraOffset;
            }
        }
    }

    private void OnCollisionStay(Collision collisionObject) //collision check for walls and stop if hit
    {
        if (collisionObject.gameObject.tag == "Wall")
        {
            momentum = 0;
        }
    }

    public void ShootShotGun()
    {
        if (gunRotation.shotGunOut)
        {
            if (Input.GetMouseButtonUp(1) && shot == false) //checks to see if space has been pressed
            {
                Debug.Log("mouse button pressed");
                momentum = rb.velocity.x;
                Instantiate(projectilePrefab, launchOffset.position, gun.transform.rotation); //if it has been then we instantiate the projectile at the position of the launch offset with the same rotation as the player

                shot = true;
                shooting = true;
                StartCoroutine(ShotGunCoolDown());
                StartCoroutine(MomentumSwitch());
            }
        }
    }

    public IEnumerator MomentumSwitch()
    {
        yield return new WaitForSeconds((float)0.3);
        shooting = false;
    }

    public IEnumerator ShotGunCoolDown()
    {
        yield return new WaitForSeconds(2);
        shot = false;
    }
    public void Attack()
    {
        // Increases the punch number to change the animation
        punchNumber++;
        // If the number exceeds the number of animations reset it to the first animation
        if (punchNumber > 2)
        {
            punchNumber = 1;
        }
        // If it has been too long since the last attack reset the animation to the first attack animation
        if (timeSinceAttack > 1.5f)
        {
            punchNumber = 1;
        }
        // Plays the attack animation and changes depending on the attack chain
        animator.SetTrigger("Attack" + punchNumber);

        //Detect enemies
        Collider[] enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        Collider[] walls = Physics.OverlapSphere(attackPoint.position, attackRange, wallLayer);

        //Damage Enemy
        foreach (Collider enemy in enemiesHit)
        {
            // Plays the punch sound
            canPunchSound = true;
            // Check to see what was hit
            Debug.Log("We Hit " + enemy.name);
            // So we can damage the enemy we need a "Take Damage" function in the enemy script 
            enemy.GetComponent<EnemyTakeDamage>().TakeDamage(attackDamage);
            
        }

        foreach(Collider wall in walls)
        {
            // Runs function to destroy the walls
            wall.GetComponent<Destructible>().Destroy();
        }

        if(enemiesHit.Length == 0)
        {
            canPunchSound = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Check player health function.
    private void CheckPlayerHealth()
    {
        healthBar.SetHealth((int)currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died.");
            PlayerDeath();
        }
    }

    //Player has died function
    private void PlayerDeath()
    {
        if (reset == null)
        {
            rb.velocity = Vector3.zero;
            transform.position = respawnPoint; //moves player location to the respawn location.
            currentHealth = maxHealth;
        }
        else
        {
            reset.ResetSceneState();
        }
        
    }

    // Setting respawn loaction after colliding with checkpoint.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position; //sets the respawn location to position of collision.

            newCheckPoint = collision.gameObject;

            Destroy(lastCheckPoint.gameObject);
        }

        if (collision.tag == "DeathTrigger")
        {
            PlayerDeath();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (newCheckPoint != null)
        {
            lastCheckPoint = newCheckPoint;
        }
        newCheckPoint.GetComponent<Collider>().enabled = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
        }
    }

    public IEnumerator PunchReset()
    {
        yield return new WaitForSeconds(2);
        punchNumber = 0;
    }

    public void SlowMo()
    {
        if(Input.GetMouseButtonDown(1) && !isGrounded)
        {
            if(shot == false)
            {
                Time.timeScale = 0.2f;
            }
        }
    }

    public void ResetSlowMo()
    {
        if(Input.GetMouseButtonUp(1) && !shot)
        {
            rb.velocity = new Vector3(0, 0, 0);
            Time.timeScale = 1;
        }
    }
    /// <summary>
    /// Function that draws a line to show the player where they will land with a rocket jump.
    /// </summary>
    private void DrawProjection()
    {
        // Enables the line renderer
        trajectoryLine.enabled = true;
        // Calculates the amount of points on the line
        trajectoryLine.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        // Sets the start posisiton for the line renderer 
        Vector3 startPos = releasePos.position;
        // Gets the intial velocity of the player
        Vector3 startVeloctiy = projectileForce * -releasePos.right / rb.mass;
        // Points on the line
        int i = 0;
        // Sets the points on the line
        trajectoryLine.SetPosition(i, startPos);   
        for (float time = 0; time < linePoints; time += timeBetweenPoints) 
        {
            i++;
            // Sets the x & z point for the line 
            Vector3 point = startPos + time * startVeloctiy;
            // Sets the y point for the line
            point.y = startPos.y + startVeloctiy.y * time + (Physics.gravity.y / 2f * time * time);
            // Adds the point to the line
            trajectoryLine.SetPosition(i, point);  
        }
    }
}
