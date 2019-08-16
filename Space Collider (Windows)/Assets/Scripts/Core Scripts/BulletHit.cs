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
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.takeDamage(damage);
                if (explosion)
                {
                    GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                    if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                }
                Destroy(gameObject);
            }
        } else if (other.CompareTag("Shield"))
        {
            ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
            if (shieldHealth)
            {
                shieldHealth.takeDamage(damage);
                if (explosion)
                {
                    GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                    if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                }
                Destroy(gameObject);
            }
        }
    }
}