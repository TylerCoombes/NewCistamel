using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public Transform player;

    public bool isFlipped = false;

    public float radius;
    public LayerMask playerMask;
    public float damage;
    public float explosionForce;

    public Animator animator;

    public EnemyTakeDamage enemyTakeDamage;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyTakeDamage = GetComponent<EnemyTakeDamage>();
    }

    public void Update()
    {
        if (enemyTakeDamage.currentHealth <= 0)
        {
            Die();
        }
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x < player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x > player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void ExplodeDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, playerMask);

        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<PlayerController>().currentHealth -= damage;
            hitCollider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, radius);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(transform.gameObject);
    }

    public void Die()
    {
        animator.SetTrigger("Attack");
    }
}
