using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGasDamage : MonoBehaviour
{
    // Gas causes damage to enemies as a default.
    [SerializeField] private bool canCauseDamage = true;
    public EnemyTakeDamage enemyTakeDamage;

    public void Start()
    {
        enemyTakeDamage = GetComponent<EnemyTakeDamage>();
    }
    public void Update()
    {
        if (enemyTakeDamage.currentHealth <= 0)
        {
            Die();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        float timeToKill = 2; // Can change this variable to change how long it takes enemy to die in gas.
        if (other.CompareTag ("Enemy"))
        {
            Destroy(other, timeToKill);
            Debug.Log("Enemy has fallen into gas.");
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
