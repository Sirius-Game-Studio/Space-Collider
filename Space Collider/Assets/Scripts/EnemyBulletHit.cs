using UnityEngine;

public class EnemyBulletHit : MonoBehaviour
{
    [Tooltip("Amount of damage dealt to players (only affects armor).")] public long damage = 1;
    [Tooltip("Damage multiplier on Hard mode (only applies to Endless Mode).")] public float hardDamageMultiplier = 1;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject playerExplosion;

    private GameController gameController;

    void Awake()
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
                    if (explosion)
                    {
                        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                        if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                    }
                    if (playerController.armor <= 0 && playerExplosion)
                    {
                        GameObject newExplosion = Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                        if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                    }
                    playerController.onHit(damage, false);
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