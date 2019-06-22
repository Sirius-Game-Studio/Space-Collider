using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private GameObject playerExplosion;

    private GameController gameController;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!gameController.won)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            ShieldHealth shieldHealth = other.GetComponent<ShieldHealth>();
            if (other.CompareTag("Player") && playerController)
            {
                if (playerExplosion)
                {
                    GameObject newExplosion = Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
                }
                playerController.onHit(9223372036854775807, true);
            } else if (other.CompareTag("Shield") && shieldHealth)
            {
                shieldHealth.takeDamage(9223372036854775807);
            }
        }
    }
}