using UnityEngine;

public class EnemyBulletHit : MonoBehaviour
{
    [Tooltip("How much damage this projectile deals to the player's Ship Armor.")] public long damage = 1;
    [Tooltip("Damage multiplier on Hard mode (Survival only).")] public float hardDamageMultiplier = 1;
    [SerializeField] private GameObject explosion = null;

    void Start()
    {
        if (!GameController.instance.isCampaignLevel && GameController.instance.difficulty >= 3) damage = (long)(damage * hardDamageMultiplier);
    }

    void Update()
    {
        if (damage < 1) damage = 1; //Checks if damage is below 1
    }

    void OnTriggerStay(Collider other)
    {
        if (!GameController.instance.gameOver)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
            if (other.CompareTag("Player") && playerController && !playerController.invulnerable)
            {
                playerController.onHit(damage, false);
                if (explosion) Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            } else if (other.CompareTag("Shield") && shieldHealth)
            {
                shieldHealth.takeDamage(damage);
                if (explosion) Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}