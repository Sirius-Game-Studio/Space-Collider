using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public long health = 0;
    [Tooltip("Amount of score earned from killing this enemy (Survival only).")] [SerializeField] private long score = 0;

    [Header("Additional Enemy Spawning")]
    public bool spawnEnemiesOnDeath = false;
    [SerializeField] private long enemyAmount = 0;
    [SerializeField] private GameObject[] enemiesToSpawn = new GameObject[0];
    [SerializeField] private Vector2 randomSpawnX = Vector2.zero;
    [SerializeField] private Vector2 randomSpawnY = Vector2.zero;

    [Header("Extra Life")]
    [Tooltip("Survival only.")] [SerializeField] private bool giveLivesOnDeath = false;
    [Tooltip("Amount of lives given upon killing this enemy (Survival only).")] [SerializeField] private long livesGiven = 1;

    [Header("Setup")]
    [SerializeField] private GameObject explosion = null;

    void Update()
    {
        if (health <= 0)
        {
            GameController.instance.addScore(score);
            if (explosion) Instantiate(explosion, transform.position, transform.rotation);
            if (spawnEnemiesOnDeath && enemiesToSpawn.Length > 0 && enemyAmount > 0)
            {
                for (int i = 0; i < enemyAmount; i++)
                {
                    Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], transform.position + new Vector3(Random.Range(randomSpawnX.x, randomSpawnX.y), Random.Range(randomSpawnY.x, randomSpawnY.y), 0), Quaternion.Euler(Random.Range(-180, 180), -90, 90));
                }
            }
            if (!GameController.instance.isCampaignLevel && giveLivesOnDeath && livesGiven > 0)
            {
                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController && playerController.lives > 0)
                {
                    if (GameController.instance.difficulty < 4) //If current Survival difficulty is below NIGHTMARE!
                    {
                        giveLife(playerController);
                    } else //If current Survival difficulty is NIGHTMARE!
                    {
                        ++GameController.instance.killsForLife;
                        if (GameController.instance.killsForLife >= 2)
                        {
                            giveLife(playerController);
                            GameController.instance.killsForLife = 0;
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    public void takeDamage(long damage)
    {
        if (damage > 0)
        {
            health -= damage;
        } else
        {
            --health;
        }
    }

    void giveLife(PlayerController playerController)
    {
        if (playerController && playerController.lives > 0 && livesGiven > 0 && health <= 0)
        {
            playerController.lives += livesGiven;
            if (livesGiven == 1)
            {
                GameController.instance.showMessage("You got 1 Life!");
            } else if (livesGiven > 1)
            {
                GameController.instance.showMessage("You got " + livesGiven + " Lives!");
            }
        }
    }
}