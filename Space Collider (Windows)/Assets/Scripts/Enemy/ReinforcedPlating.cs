using UnityEngine;

/*
Reinforced Plating (NIGHTMARE!-Only Ability):
Increases health by 1. When killed, the armor plating breaks up into 6 pieces.
*/

public class ReinforcedPlating : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private GameController gameController;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        gameController = FindObjectOfType<GameController>();
        if (!CompareTag("Enemy"))
        {
            Debug.LogError("GameObject " + name + " is not tagged as Enemy!");
            return;
        }
        if (gameController.isStandard) return;
        if (!gameController.isStandard && gameController.difficulty < 4) return;
        ability();
    }

    void ability() //Ability code
    {
        ++enemyHealth.health;
        enemyHealth.spawnEnemiesOnDeath = true;
    }
}
