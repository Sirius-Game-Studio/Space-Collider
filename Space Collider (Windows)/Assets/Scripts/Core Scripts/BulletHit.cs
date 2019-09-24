using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public long damage = 1;
    [SerializeField] private GameObject explosion = null;

    private bool destroy = false;

    void Update()
    {
        if (destroy)
        {
            if (explosion)
            {
                GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
            }
            Destroy(gameObject);
        }
        if (damage < 1) damage = 1; //Checks if damage is below 1
    }

    void OnTriggerStay(Collider other)
    {
        if (!destroy)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth)
                {
                    enemyHealth.takeDamage(damage);
                    destroy = true;
                }
            } else if (other.CompareTag("Shield"))
            {
                ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
                if (shieldHealth)
                {
                    shieldHealth.takeDamage(damage);
                    destroy = true;
                }
            }
        }
    }
}