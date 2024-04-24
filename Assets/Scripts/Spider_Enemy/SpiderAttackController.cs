using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttackController : MonoBehaviour
{
    public Transform player;
    public Transform attackPosition;

    public bool isFlipped = false;

    public float radius;
    public LayerMask playerMask;
    public float damage;

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

    public void AttackDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPosition.position, radius, playerMask);

        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<PlayerController>().currentHealth -= damage;
        }
    }

    public void DestroyEnemy()
    {
        Destroy(transform.gameObject);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPosition.position, radius);
    }
}
