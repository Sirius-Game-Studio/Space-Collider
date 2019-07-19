using UnityEngine;

public class TouchPowerup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController)
            {
                if (CompareTag("Multishot"))
                {
                    playerController.multishot();
                } else if (CompareTag("IncreasedDamage"))
                {
                    playerController.increasedDamage();
                } else if (CompareTag("ShipArmor"))
                {
                    playerController.shipArmor();
                } else if (CompareTag("FasterFireRate"))
                {
                    playerController.fasterFiring();
                } else if (CompareTag("FasterSpeed"))
                {
                    playerController.fasterSpeed();
                } else
                {
                    Debug.LogError("Powerup tag " + tag + " is invalid.");
                }
                Destroy(gameObject);
            } else
            {
                Debug.LogError("Could not find PlayerController!");
            }
        }
    }
}