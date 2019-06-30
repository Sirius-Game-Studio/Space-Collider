using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Amount of health this enemy starts with.")] public long health = 0;
    [Tooltip("Amount of score this enemy gives to the player upon death.")] [SerializeField] private int score = 0;
    [Tooltip("The kills key to set (leave blank to not count towards kills).")] [SerializeField] private string killsKey = "";

    [Header("Additional Enemy Spawning")]
    [Tooltip("Whether if this enemy spawns additional enemies upon death or not.")] public bool spawnEnemiesOnDeath = false;
    [Tooltip("List of enemies to spawn.")] [SerializeField] private GameObject[] enemiesToSpawn;
    [Tooltip("Amount of enemies this enemy spawns upon death.")] [SerializeField] private long enemyAmount = 0;
    [SerializeField] private Vector2 randomSpawnX = Vector2.zero;
    [SerializeField] private Vector2 randomSpawnZ = Vector2.zero;

    [Header("Extra Life")]
    [Tooltip("Whether if this enemy gives lives to the player upon death or not. (only in Endless Mode)")] [SerializeField] private bool giveLivesOnDeath = false;
    [Tooltip("Amount of lives this enemy gives to the player upon death. (only in Endless Mode)")] [SerializeField] private long livesGiven = 1;

    [Header("Setup")]
    [SerializeField] private GameObject explosion;

    private GameController gameController;

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (health <= 0)
        {
            gameController.addScore(score);
            if (explosion)
            {
                GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
                if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
            }
            if (spawnEnemiesOnDeath && enemiesToSpawn.Length > 0 && enemyAmount > 0)
            {
                for (int i = 0; i < enemyAmount; i++)
                {
                    Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], transform.position + new Vector3(Random.Range(randomSpawnX.x, randomSpawnX.y), Random.Range(randomSpawnZ.x, randomSpawnZ.y), 0), Quaternion.Euler(Random.Range(-180, 180), -90, 90));
                }
            }
            if (!gameController.isStandard && giveLivesOnDeath && livesGiven >= 1)
            {
                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController && playerController.lives > 0 && playerController.lives < playerController.maxLives)
                {
                    if (gameController.difficulty < 4)
                    {
                        giveLife(playerController);
                    } else
                    {
                        ++gameController.killsForLife;
                        if (gameController.killsForLife >= 2)
                        {
                            giveLife(playerController);
                            gameController.killsForLife = 0;
                        }
                        print(gameController.killsForLife);
                    }
                }
            }
            if (killsKey != "")
            {
                int kill = PlayerPrefs.GetInt(killsKey);
                if (kill <= 0)
                {
                    kill = 1;
                } else
                {
                    ++kill;
                }
                PlayerPrefs.SetInt(killsKey, kill);
                PlayerPrefs.Save();
            }
            gameObject.SetActive(false); //Makes the enemy inactive, also ensuring it doesn't shoot out of nowhere
            Destroy(gameObject);
        }
        if (livesGiven < 1) livesGiven = 1; //Checks if livesGiven is below 1
    }

    void giveLife(PlayerController playerController)
    {
        if (playerController && playerController.lives > 0 && playerController.lives < playerController.maxLives && livesGiven > 0)
        {
            playerController.lives += livesGiven;
            if (livesGiven == 1)
            {
                gameController.showMessage("You got 1 Life!");
            } else if (livesGiven > 1)
            {
                gameController.showMessage("You got " + livesGiven + " Lives!");
            }
        }
    }

    public void takeDamage(long damage)
    {
        if (damage > 0)
        {
            health -= damage;
        } else
        {
            --damage;
        }
    }
}