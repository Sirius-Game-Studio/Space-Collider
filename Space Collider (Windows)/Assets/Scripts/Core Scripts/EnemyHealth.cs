using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public long health = 0;
    [Tooltip("The amount of score added when killed.")] [SerializeField] private long score = 0;
    [Tooltip("The kills key to update (leave blank to not count towards kills).")] [SerializeField] private string killsKey = "";

    [Header("Additional Enemy Spawning")]
    public bool spawnEnemiesOnDeath = false;
    [Tooltip("The amount of enemies spawned on death.")] [SerializeField] private long enemyAmount = 0;
    [SerializeField] private GameObject[] enemiesToSpawn = new GameObject[0];
    [SerializeField] private Vector2 randomSpawnX = Vector2.zero;
    [SerializeField] private Vector2 randomSpawnY = Vector2.zero;

    [Header("Extra Life")]
    [Tooltip("Only works in Survival Mode.")] [SerializeField] private bool giveLivesOnDeath = false;
    [Tooltip("Only works in Survival Mode.")] [SerializeField] private long livesGiven = 1;

    [Header("Setup")]
    [SerializeField] private GameObject explosion = null;

    void Start()
    {
        if (health <= 0) die();
    }

    void Update()
    {
        if (livesGiven < 1) livesGiven = 1; //Checks if livesGiven is below 1
    }

    void die()
    {
        if (health <= 0)
        {
            GameController.instance.addScore(score);
            if (explosion)
            {
                GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
                if (newExplosion.GetComponent<AudioSource>()) newExplosion.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
            }
            if (spawnEnemiesOnDeath && enemiesToSpawn.Length > 0 && enemyAmount > 0)
            {
                for (int i = 0; i < enemyAmount; i++)
                {
                    Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], transform.position + new Vector3(Random.Range(randomSpawnX.x, randomSpawnX.y), Random.Range(randomSpawnY.x, randomSpawnY.y), 0), Quaternion.Euler(Random.Range(-180, 180), -90, 90));
                }
            }
            if (!GameController.instance.isStandard && giveLivesOnDeath && livesGiven >= 1)
            {
                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController && playerController.lives > 0 && playerController.lives < playerController.maxLives)
                {
                    if (GameController.instance.difficulty < 4) //If current Survival Mode difficulty is below NIGHTMARE!
                    {
                        giveLife(playerController);
                    } else //If current Survival Mode difficulty is NIGHTMARE!
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
            if (killsKey != "")
            {
                if (!PlayerPrefs.HasKey(killsKey))
                {
                    PlayerPrefs.SetString(killsKey, "1");
                } else
                {
                    /*
                    long update = long.Parse(PlayerPrefs.GetString(killsKey));
                    ++update;
                    PlayerPrefs.SetString(killsKey, update.ToString());
                    */
                }
                PlayerPrefs.Save();
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
        if (health <= 0) die();
    }

    void giveLife(PlayerController playerController)
    {
        if (playerController && playerController.lives > 0 && playerController.lives < playerController.maxLives && livesGiven > 0 && health <= 0)
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