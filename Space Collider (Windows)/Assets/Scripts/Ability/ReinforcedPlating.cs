using UnityEngine;

public class ReinforcedPlating : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (!GameController.instance.isCampaignLevel && GameController.instance.difficulty >= 4) main();
    }

    void main()
    {
        ++enemyHealth.health;
        enemyHealth.spawnEnemiesOnDeath = true;
        enabled = false;
    }
}