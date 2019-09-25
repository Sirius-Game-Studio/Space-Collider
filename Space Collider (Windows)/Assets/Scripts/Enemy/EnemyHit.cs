using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (!GameController.instance.gameOver)
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