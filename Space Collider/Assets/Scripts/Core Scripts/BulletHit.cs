using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [Tooltip("Amount of damage dealt to enemies.")] public long damage = 1;

    void Update()
    {
        if (damage < 1) damage = 1; //Checks if damage is below 1
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.takeDamage(damage);
                Destroy(gameObject);
            }
        } else if (other.CompareTag("Shield"))
        {
            ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
            if (shieldHealth)
            {
                shieldHealth.takeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}