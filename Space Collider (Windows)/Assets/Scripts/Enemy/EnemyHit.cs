using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private GameController gameController;

    void Start()
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
                playerController.onHit(9223372036854775807, true);
            } else if (other.CompareTag("Shield") && shieldHealth)
            {
                shieldHealth.takeDamage(9223372036854775807);
            }
        }
    }
}