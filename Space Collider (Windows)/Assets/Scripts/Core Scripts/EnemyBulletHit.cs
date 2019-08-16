using UnityEngine;

public class EnemyBulletHit : MonoBehaviour
{
    [Tooltip("The amount of damage dealt to the player's Ship Armor.")] public long damage = 1;
    [Tooltip("Damage multiplier on Hard mode (only applies to Survival Mode).")] public float hardDamageMultiplier = 1;
    [SerializeField] private GameObject explosion = null;

    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        if (!gameController.isStandard && gameController.difficulty >= 3) damage = (long)(damage * hardDamageMultiplier);
    }

    void Update()
    {
        if (damage < 1) damage = 1; //Checks if damage is below 1
    }

    void OnTriggerStay(Collider other)
    {
        if (!gameController.won)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
            if (other.CompareTag("Player") && playerController)
            {
                if (!playerController.invulnerable)
                {
                    playerController.onHit(damage, false);
                    if (explosion)
                    {
                        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                        if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                    }
                    Destroy(gameObject);
                }
            } else if (other.CompareTag("Shield") && shieldHealth)
            {
                if (explosion)
                {
                    GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                    if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                }
                shieldHealth.takeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}