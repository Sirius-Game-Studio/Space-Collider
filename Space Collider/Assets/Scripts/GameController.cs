using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game Settings")]
    [Tooltip("Amount of waves needed to clear for a shield group respawn (set to 0 to disable respawning).")] [SerializeField] private long wavesToClearForShieldRespawn = 0;
    [Tooltip("How long it takes for aliens to move (X is current, Y is maximum).")] public Vector2 moveTime = new Vector2(0.3f, 0.12f);
    [Tooltip("How long it takes for aliens to shoot (X is current, Y is maximum).")] public Vector2 fireRate = new Vector2(3, 0.6f);
    [Tooltip("Amount of speed to add to enemy bullets (X is current, Y is maximum).")] public Vector2 bulletSpeedIncrement = new Vector2(0, 2.5f);
    [Tooltip("How long it takes for a enemy ship to spawn.")] public Vector2 enemyShipSpawnTime = new Vector2(45, 60);
    [Tooltip("How long it takes for asteroids to spawn.")] [SerializeField] private Vector2 asteroidSpawnTime = new Vector2(1, 8);
    [Tooltip("How long it takes for powerups to spawn.)")] [SerializeField] private Vector2 powerupTime = new Vector2(12, 18);
    [Tooltip("Can independent alien groups spawn?")] [SerializeField] private bool canSeparateEnemiesSpawn = true;
    [Tooltip("Can enemy ships spawn?")] [SerializeField] private bool canEnemyShipsSpawn = true;
    [Tooltip("Can asteroids spawn?")] [SerializeField] private bool canAsteroidsSpawn = true;
    [Tooltip("The shield group to respawn after a set amount of waves.")] [SerializeField] private GameObject shieldGroupToRespawn;
    [Tooltip("List of alien groups to spawn each wave.")] [SerializeField] private GameObject[] enemyPatterns;
    [Tooltip("List of alien groups to spawn when a enemy reaches a certain point.")] [SerializeField] private GameObject[] separateEnemyPatterns;
    [Tooltip("Which enemy ship to spawn.")] [SerializeField] private GameObject enemyShip;
    [Tooltip("List of asteroids to spawn.")] [SerializeField] private GameObject[] asteroids;
    [Tooltip("List of powerups to spawn.")] [SerializeField] private GameObject[] powerups;

    [Header("NIGHTMARE!-Only Settings")]
    [Tooltip("List of bosses to spawn each 5th wave (only used in NIGHTMARE!).")] [SerializeField] private GameObject[] NIGHTMAREBosses;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip gameOverJingle;
    [SerializeField] private AudioClip winJingle;
    [SerializeField] private AudioClip clickSound;

    [Header("UI")]
    [SerializeField] private Canvas gamePausedUI;
    [SerializeField] private Canvas gameOverUI;
    [SerializeField] private Canvas levelCompletedUI;
    [SerializeField] private Canvas restartUI;
    [SerializeField] private Canvas exitToMainMenuUI;
    [SerializeField] private Canvas quitGameUI;
    [SerializeField] private Canvas settingsUI;
    [SerializeField] private Canvas graphicsQualityUI;
    [SerializeField] private Canvas soundUI;
    [SerializeField] private Text livesCount;
    [SerializeField] private Text levelCount;
    [SerializeField] private Text scoreCount;
    [SerializeField] private Text waveCount;
    [SerializeField] private Text fullscreenText;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Text highScoreIndicator;
    [SerializeField] private Text message;

    [Header("Miscellanous")]
    [Tooltip("Should this scene act like a Standard Mode level?")] public bool isStandard = false;
    [Tooltip("1 is Easy, 2 is Normal, 3 is Hard, 4 is NIGHTMARE! (only in Endless Mode).")] [Range(1, 4)] public int difficulty = 1;
    [Tooltip("Gives a life after the kill goal is reached (only used in NIGHTMARE).")] public int killsForLife = 0;
    public bool gameOver = false;
    public bool won = false;
    public bool paused = false;

    [Header("Setup")]
    [SerializeField] private GameObject enemyHolder;
    [SerializeField] private GameObject shieldGroup;

    private AudioSource audioSource;
    private PlayerController playerController;
    private LevelHolder levelHolder;
    private int score = 0;
    private long wave = 1;
    private bool enemySpawnTrigger = false; //Checks if a separate enemy pattern was spawned
    private float enemySpawnZPosition = -3; //The Z position enemies must be in to spawn a separate enemy pattern
    private bool loading = false;
    private long wavesCleared = 0; //Used for when the shields should be respawned
    private long wavesTillBoss = 6; //Only used in Endless Mode NIGHTMARE!
    private int clickSource = 1; //1 is pause menu, 2 is game over menu, 3 is level completed menu
    private bool playedLoseJingle = false, playedWinJingle = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();
        levelHolder = FindObjectOfType<LevelHolder>();
        gameOver = false;
        enemySpawnZPosition = Random.Range(-3, -3.5f);
        killsForLife = 0;
        Time.timeScale = 1;
        paused = false;
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 1);
            PlayerPrefs.Save();
            soundSlider.value = 1;
        } else
        {
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.Save();
            musicSlider.value = 1;
        } else
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (Camera.main.GetComponent<AudioSource>())
        {
            Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
            Camera.main.GetComponent<AudioSource>().Play();
        }
        gamePausedUI.enabled = false;
        gameOverUI.enabled = false;
        levelCompletedUI.enabled = false;
        restartUI.enabled = false;
        exitToMainMenuUI.enabled = false;
        quitGameUI.enabled = false;
        spawnEnemyPattern();
        if (canEnemyShipsSpawn) StartCoroutine("spawnEnemyShips");
        if (canAsteroidsSpawn) StartCoroutine("spawnAsteroids");
        StartCoroutine("spawnPowerups");
        print("Current enemy movement time: " + moveTime);
        print("Current enemy fire rate: " + fireRate);
        print("Current separate enemy spawn trigger position: " + enemySpawnZPosition);
    }
    
    void Update()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        if (Input.GetKeyDown(KeyCode.Escape)) pause();
        if (Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
        if (enemyHolder && !gameOver && !won)
        {
            if (enemyHolder.transform.childCount > 0)
            {
                if (canSeparateEnemiesSpawn)
                {
                    foreach (Transform pattern in enemyHolder.transform)
                    {
                        foreach (Transform enemy in pattern)
                        {
                            if (enemy.CompareTag("Enemy") && separateEnemyPatterns.Length > 0 && !enemySpawnTrigger && enemy.position.z <= enemySpawnZPosition)
                            {
                                GameObject separateEnemies = Instantiate(separateEnemyPatterns[Random.Range(0, separateEnemyPatterns.Length)], new Vector3(0, 0, 19.5f), Quaternion.Euler(0, 0, 0));
                                separateEnemies.transform.SetParent(enemyHolder.transform);
                                enemySpawnTrigger = true;
                                print("Spawned separate enemy pattern " + separateEnemies.name + ".");
                            }
                        }
                    }
                }
            } else if (enemyHolder.transform.childCount <= 0)
            {
                if (isStandard)
                {
                    if (wave < enemyPatterns.Length)
                    {
                        ++wave;
                        ++wavesCleared;
                        print("Waves to clear till shield respawn: " + wavesCleared + "/" + wavesToClearForShieldRespawn);
                    } else
                    {
                        won = true;
                        if (!PlayerPrefs.HasKey("Wins"))
                        {
                            PlayerPrefs.SetInt("Wins", 1);
                        } else
                        {
                            int wins = PlayerPrefs.GetInt("Wins");
                            ++wins;
                            PlayerPrefs.SetInt("Wins", wins);
                        }
                        if (levelHolder && levelHolder.level >= levelHolder.maxLevels)
                        {
                            levelHolder.level = 1;
                            PlayerPrefs.SetInt("StandardLevel", 1);
                            PlayerPrefs.Save();
                            levelCompletedUI.enabled = false;
                            if (!loading)
                            {
                                loading = true;
                                SceneManager.LoadSceneAsync("Standard Mode Ending");
                            }
                        }
                    }
                } else
                {
                    ++wave;
                    ++wavesCleared;
                    if (difficulty >= 4) ++wavesTillBoss;
                    print("Waves to clear till shield respawn: " + wavesCleared + "/" + wavesToClearForShieldRespawn);
                }
                if (!gameOver && !won && !isStandard)
                {
                    if (!isStandard)
                    {
                        if (difficulty <= 1)
                        {
                            moveTime -= new Vector2(0.015f, 0);
                            fireRate -= new Vector2(0.15f, 0);
                            bulletSpeedIncrement -= new Vector2(0.0875f, 0);
                        } else if (difficulty == 2)
                        {
                            moveTime -= new Vector2(0.02f, 0);
                            fireRate -= new Vector2(0.2f, 0);
                            bulletSpeedIncrement += new Vector2(0.125f, 0);
                        } else if (difficulty == 3)
                        {
                            moveTime -= new Vector2(0.025f, 0);
                            fireRate -= new Vector2(0.3f, 0);
                            bulletSpeedIncrement -= new Vector2(0.15f, 0);
                        } else if (difficulty >= 4)
                        {
                            moveTime -= new Vector2(0.03f, 0);
                            fireRate -= new Vector2(0.4f, 0);
                            bulletSpeedIncrement -= new Vector2(0.2f, 0);
                        }
                    }
                    enemySpawnTrigger = false; //Resets the enemy spawn trigger check
                    enemySpawnZPosition = Random.Range(-3, -3.5f); //Resets the separate enemy spawn trigger position
                    spawnEnemyPattern();
                    print("Current enemy fire rate: " + fireRate);
                    print("Current separate enemy spawn trigger position: " + enemySpawnZPosition);
                    print("Current enemy movement time: " + moveTime);
                    if (!isStandard && difficulty >= 4) print("Waves till boss spawn: " + wavesTillBoss);
                }
            }
        }
        if (shieldGroupToRespawn && wavesToClearForShieldRespawn > 0 && wavesCleared >= wavesToClearForShieldRespawn)
        {
            wavesCleared = 0;
            if (shieldGroup)
            {
                Destroy(shieldGroup);
                shieldGroup = Instantiate(shieldGroupToRespawn, new Vector3(0, -4.5f, 0), Quaternion.Euler(-90, 0, 0));
            } else
            {
                shieldGroup = Instantiate(shieldGroupToRespawn, new Vector3(0, -4.5f, 0), Quaternion.Euler(-90, 0, 0));
            }
        }
        if (gameOver && !won)
        {
            if (!restartUI.enabled && !exitToMainMenuUI.enabled && !quitGameUI.enabled) gameOverUI.enabled = true;
            clickSource = 2;
            if (!playedLoseJingle)
            {
                playedLoseJingle = true;
                audioSource.PlayOneShot(gameOverJingle, PlayerPrefs.GetFloat("SoundVolume"));
            }
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            if (!isStandard)
            {
                if (difficulty <= 1)
                {
                    setNewHighScore("EasyHighScore");
                } else if (difficulty == 2)
                {
                    setNewHighScore("NormalHighScore");
                } else if (difficulty == 3)
                {
                    setNewHighScore("HardHighScore");
                } else if (difficulty >= 4)
                {
                    setNewHighScore("NightmareHighScore");
                }
                PlayerPrefs.Save();
            }
        } else
        {
            gameOverUI.enabled = false;
        }
        if (!isStandard && !gameOver && won)
        {
            if (!restartUI.enabled && !exitToMainMenuUI.enabled && !quitGameUI.enabled) levelCompletedUI.enabled = true;
            clickSource = 3;
            if (!playedWinJingle)
            {
                playedWinJingle = true;
                audioSource.PlayOneShot(winJingle, PlayerPrefs.GetFloat("SoundVolume"));
            }
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
        } else
        {
            levelCompletedUI.enabled = false;
        }
        if (livesCount) livesCount.text = playerController.lives + "/" + playerController.maxLives;
        if (scoreCount)
        {
            scoreCount.text = score.ToString();
            if (isStandard)
            {
                scoreCount.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5000, 0);
            } else
            {
                scoreCount.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -5);
            }
        }
        if (levelCount && levelHolder)
        {
            levelCount.text = levelHolder.level.ToString();
            if (isStandard)
            {
                levelCount.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -5);
            } else
            {
                levelCount.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5000, 0);
            }
        }
        if (waveCount)
        {
            if (isStandard)
            {
                waveCount.text = wave + "/" + enemyPatterns.Length;
            } else
            {
                waveCount.text = wave.ToString();
            }
        }
        if (Screen.fullScreen)
        {
            fullscreenText.text = "Change to Windowed Mode";
            fullscreenText.rectTransform.sizeDelta = new Vector2(294, 23);
        }
        else
        {
            fullscreenText.text = "Change to Fullscreen";
            fullscreenText.rectTransform.sizeDelta = new Vector2(223, 23);
        }
        if (moveTime.x < moveTime.y) moveTime = new Vector2(moveTime.y, moveTime.y); //Checks if moveTime is exceeding the maximum
        if (fireRate.x < fireRate.y) fireRate = new Vector2(fireRate.y, fireRate.y); //Checks if fireRate is exceeding the maximum
        if (bulletSpeedIncrement.x > bulletSpeedIncrement.y) bulletSpeedIncrement = new Vector2(bulletSpeedIncrement.y, bulletSpeedIncrement.y); //Checks if bulletSpeedIncrement is exceeding the maximum
        if (difficulty < 4) //Checks if current Endless Mode difficulty isn't NIGHTMARE!
        {
            NIGHTMAREBosses = new GameObject[0];
            killsForLife = 0;
        }
    }

    void spawnEnemyPattern()
    {
        GameObject pattern;
        if (isStandard)
        {
            pattern = Instantiate(enemyPatterns[wave - 1], new Vector3(0, 19.5f, 0), enemyHolder.transform.rotation);
        } else
        {
            if (difficulty < 4)
            {
                pattern = Instantiate(enemyPatterns[Random.Range(0, enemyPatterns.Length)], new Vector3(0, 19.5f, 0), enemyHolder.transform.rotation);
            } else
            {
                if (wavesTillBoss < 5)
                {
                    pattern = Instantiate(enemyPatterns[Random.Range(0, enemyPatterns.Length)], new Vector3(0, 19.5f, 0), enemyHolder.transform.rotation);
                    if (!pattern.CompareTag("ExcludeAlwaysFast"))
                    {
                        pattern.GetComponent<EnemyMover>().fast = true;
                        pattern.GetComponent<EnemyGun>().fast = true;
                    }
                } else
                {
                    pattern = Instantiate(NIGHTMAREBosses[Random.Range(0, NIGHTMAREBosses.Length)], new Vector3(0, 19.5f, 0), enemyHolder.transform.rotation);
                    if (!pattern.CompareTag("ExcludeAlwaysFast"))
                    {
                        pattern.GetComponent<EnemyMover>().fast = true;
                        pattern.GetComponent<EnemyGun>().fast = true;
                    }
                    wavesTillBoss = 0;
                }
            }
        }
        pattern.transform.SetParent(enemyHolder.transform);
        print("Current enemy pattern: " + pattern.name);
    }

    IEnumerator spawnEnemyShips()
    {
        while (!gameOver && !won && enemyShip)
        {
            if (!paused)
            {
                yield return new WaitForSeconds(Random.Range(enemyShipSpawnTime.x, enemyShipSpawnTime.y));
                if (!gameOver && !won && !paused)
                {
                    GameObject ship;
                    float randomPosition = Random.Range(Mathf.Floor(1), Mathf.Floor(2));
                    if (randomPosition < 1.5f)
                    {
                        ship = Instantiate(enemyShip, new Vector3(-18, 7.5f, 0), Quaternion.Euler(-90, 0, 0));
                        if (difficulty <= 1) ship.GetComponent<EnemyHealth>().health = 1;
                        ship.GetComponent<Mover>().speed = 5;
                    } else if (randomPosition > 1.5f)
                    {
                        ship = Instantiate(enemyShip, new Vector3(18, 7.5f, 0), Quaternion.Euler(-90, 0, 0));
                        if (difficulty <= 1) ship.GetComponent<EnemyHealth>().health = 1;
                        ship.GetComponent<Mover>().speed = -5;
                    }
                }
            } else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator spawnAsteroids()
    {
        while (!gameOver && !won)
        {
            if (!paused)
            {
                yield return new WaitForSeconds(Random.Range(asteroidSpawnTime.x, asteroidSpawnTime.y));
                if (!gameOver && !won && !paused)
                {
                    Instantiate(asteroids[Random.Range(0, asteroids.Length)], new Vector3(Random.Range(-10.5f, 10.5f), 9.5f, 0), Quaternion.Euler(-90, 90, -90));
                }
            } else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator spawnPowerups()
    {
        while (!gameOver && !won)
        {
            if (!paused)
            {
                yield return new WaitForSeconds(Random.Range(powerupTime.x, powerupTime.y));
                if (!gameOver && !won && !paused)
                {
                    Instantiate(powerups[Random.Range(0, powerups.Length)], new Vector3(Random.Range(-11, 11), 9, 0), Quaternion.Euler(0, 0, 0));
                }
            } else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void addScore(int value)
    {
        if (!isStandard && !gameOver && value > 0)
        {
            int addedScore = value;
            if (difficulty <= 1)
            {
                addedScore = (int)(addedScore * 0.75);
            } else if (difficulty == 3)
            {
                addedScore = (int)(addedScore * 1.15);
            } else if (difficulty >= 4)
            {
                addedScore = (int)(addedScore * 1.25);
            }
            score += addedScore;
        }
    }

    void setNewHighScore(string key)
    {
        if (key != "" && !isStandard)
        {
            if (!PlayerPrefs.HasKey(key) && score > 0)
            {
                PlayerPrefs.SetInt(key, score);
                highScoreIndicator.text = "NEW HIGH SCORE!";
                Invoke("stopHighScoreIndicator", 3);
            } else if (PlayerPrefs.HasKey(key) && score > PlayerPrefs.GetInt(key))
            {
                PlayerPrefs.SetInt(key, score);
                highScoreIndicator.text = "NEW HIGH SCORE!";
                Invoke("stopHighScoreIndicator", 3);
            }
            PlayerPrefs.Save();
        }
    }

    public void showMessage(string t)
    {
        if (message)
        {
            message.text = t;
            CancelInvoke("resetMessage");
            Invoke("resetMessage", 1);
        }
    }

    void resetMessage()
    {
        if (message) message.text = "";
    }

    void stopHighScoreIndicator()
    {
        highScoreIndicator.text = "";
    }

    public void pause()
    {
        if (!gameOver && !won && !settingsUI.enabled && !graphicsQualityUI.enabled && !soundUI.enabled && !restartUI.enabled && !exitToMainMenuUI.enabled && !quitGameUI.enabled)
        {
            clickSource = 1;
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
                gamePausedUI.enabled = true;
                if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Pause();
            } else
            {
                Time.timeScale = 1;
                paused = false;
                gamePausedUI.enabled = false;
                if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().UnPause();
            }
        }
    }

    public void resumeGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        Time.timeScale = 1;
        paused = false;
        gamePausedUI.enabled = false;
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().UnPause();
    }

    public void toNextLevel()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading && isStandard)
        {
            loading = true;
            ++levelHolder.level;
            if (levelHolder.level < levelHolder.maxLevels + 1)
            {
                PlayerPrefs.SetInt("StandardLevel", (int)levelHolder.level);
                PlayerPrefs.Save();
                SceneManager.LoadSceneAsync("Level " + levelHolder.level);
            } else
            {
                levelHolder.level = 1;
                PlayerPrefs.SetInt("StandardLevel", 1);
                PlayerPrefs.Save();
                SceneManager.LoadSceneAsync("Standard Mode Ending");
            }
        }
    }

    public void restartGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading)
        {
            loading = true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    public void clickRestart()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!restartUI.enabled)
        {
            restartUI.enabled = true;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = false;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = false;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = false;
            }
        } else
        {
            restartUI.enabled = false;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = true;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = true;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = true;
            }
        }
    }

    public void clickSettings()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!settingsUI.enabled)
        {
            settingsUI.enabled = true;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = false;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = false;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = false;
            }
        } else
        {
            settingsUI.enabled = false;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = true;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = true;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = true;
            }
        }
    }

    public void clickExitToMainMenu()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!exitToMainMenuUI.enabled)
        {
            exitToMainMenuUI.enabled = true;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = false;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = false;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = false;
            }
        } else
        {
            exitToMainMenuUI.enabled = false;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = true;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = true;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = true;
            }
        }
    }

    public void clickQuitGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!quitGameUI.enabled)
        {
            quitGameUI.enabled = true;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = false;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = false;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = false;
            }
        } else
        {
            quitGameUI.enabled = false;
            if (clickSource <= 1)
            {
                gamePausedUI.enabled = true;
            } else if (clickSource == 2)
            {
                gameOverUI.enabled = true;
            } else if (clickSource >= 3)
            {
                levelCompletedUI.enabled = true;
            }
        }
    }

    public void clickGraphicsQuality()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!graphicsQualityUI.enabled)
        {
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = true;
        }
        else
        {
            settingsUI.enabled = true;
            graphicsQualityUI.enabled = false;
        }
    }

    public void clickSoundMenu()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!soundUI.enabled)
        {
            settingsUI.enabled = false;
            soundUI.enabled = true;
        }
        else
        {
            settingsUI.enabled = true;
            soundUI.enabled = false;
        }
    }

    public void exitToMainMenu()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading)
        {
            loading = true;
            SceneManager.LoadSceneAsync("Main Menu");
        }
    }

    public void exitGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        Application.Quit();
    }

    public void clickChangeFullscreen()
    {
        if (settingsUI.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    public void changeToVeryLowQuality()
    {
        if (graphicsQualityUI.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            QualitySettings.SetQualityLevel(0, true);
        }
    }

    public void changeToLowQuality()
    {
        if (graphicsQualityUI.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            QualitySettings.SetQualityLevel(1, true);
        }
    }

    public void changeToMediumQuality()
    {
        if (graphicsQualityUI.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            QualitySettings.SetQualityLevel(2, true);
        }
    }

    public void changeToHighQuality()
    {
        if (graphicsQualityUI.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            QualitySettings.SetQualityLevel(3, true);
        }
    }

    public void changeToVeryHighQuality()
    {
        if (graphicsQualityUI.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            QualitySettings.SetQualityLevel(4, true);
        }
    }
}