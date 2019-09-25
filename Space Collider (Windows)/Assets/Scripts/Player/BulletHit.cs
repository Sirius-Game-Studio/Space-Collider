using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public long damage = 1;
    [SerializeField] private GameObject explosion = null;

    void Update()
    {
        if (damage < 1) damage = 1; //Checks if damage is below 1
    }

    void OnTriggerStay(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
        if (other.CompareTag("Enemy") && enemyHealth)
        {
            enemyHealth.takeDamage(damage);
            if (explosion) Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        } else if (other.CompareTag("Shield"))
        {
            shieldHealth.takeDamage(damage);
            if (explosion) Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}