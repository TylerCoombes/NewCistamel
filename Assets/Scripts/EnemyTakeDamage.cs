using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    public int bulletDamage;

    public SpriteRenderer spriteRenderer;
    Color origionalColor;
    public float flashTime = 0.3f;

    public int maxHealth = 100;
    public int currentHealth;
    public bool died = false;

    public HealthBar healthBar;
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        origionalColor = spriteRenderer.color;

        healthBar = this.gameObject.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Bullet Collided");
            TakeDamage(bulletDamage);
            FlashRed();
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FlashRed();
        healthBar.SetHealth(currentHealth);

        spriteRenderer.color = Color.red;
        spriteRenderer.color = Color.white;
    }

    public void FlashRed()
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        spriteRenderer.color = origionalColor;
    }
}
