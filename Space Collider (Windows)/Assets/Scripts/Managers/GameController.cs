using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game Settings")]
    [SerializeField] private long wavesToClearForShieldRespawn = 0;
    public float enemyMoveTime = 0.3f;
    public float enemyFireRate = 3;
    public float enemyBulletSpeedIncrement = 0;
    public float maxEnemyMoveTime = 0.12f;
    public float maxEnemyFireRate = 0.6f;
    public float maxEnemyBulletSpeedIncrement = 2.5f;
    [SerializeField] private Vector2 enemyShipSpawnTime = new Vector2(45, 60);
    [SerializeField] private Vector2 asteroidSpawnTime = new Vector2(1, 8);
    [SerializeField] private Vector2 powerupSpawnTime = new Vector2(12, 18);
    [SerializeField] private bool canEnemyShipsSpawn = true;
    [SerializeField] private bool canAsteroidsSpawn = true;

    [Header("Survival Mode-Only Settings")]
    [SerializeField] private float scoreMultiplier = 1;
    [Tooltip("1 is Easy, 2 is Normal, 3 is Hard, 4 is NIGHTMARE!.")] [Range(1, 4)] public int difficulty = 1;
    [Tooltip("List of bosses to spawn after clearing 5 waves (only used in NIGHTMARE!).")] [SerializeField] private GameObject[] NIGHTMAREBosses = new GameObject[0];

    [Header("Sound Effects")]
    [SerializeField] private AudioClip gameOverJingle = null;
    [SerializeField] private AudioClip winJingle = null;
    [SerializeField] private AudioClip clickSound = null;

    [Header("UI")]
    [SerializeField] private GameObject gameUIMain = null;
    [SerializeField] private Canvas gamePausedUI = null;
    [SerializeField] private Canvas gameOverUI = null;
    [SerializeField] private Canvas levelCompletedUI = null;
    [SerializeField] private Canvas restartUI = null;
    [SerializeField] private Canvas exitToMainMenuUI = null;
    [SerializeField] private Canvas quitGameUI = null;
    [SerializeField] private Canvas settingsUI = null;
    [SerializeField] private Text levelCount = null;
    [SerializeField] private Text scoreCount = null;
    [SerializeField] private Text waveCount = null;
    [SerializeField] private Slider soundSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Text highScoreIndicator = null;
    [SerializeField] private Text message = null;
    [SerializeField] private Text loadingText = null;

    [Header("Miscellanous")]
    [Tooltip("Should this scene act like a Campaign level?")] public bool isStandard = false;
    [Tooltip("Gives a life after the kill goal is reached (only used in Survival Mode NIGHTMARE!).")] public int killsForLife = 0;
    public bool gameOver = false;
    public bool won = false;
    public bool paused = false;

    [Header("Setup")]
    [SerializeField] private GameObject enemyHolder = null;
    [SerializeField] private GameObject shieldGroup = null;
    [SerializeField] private GameObject shieldGroupToRespawn = null;
    [SerializeField] private GameObject[] enemyPatterns = new GameObject[0];
    [SerializeField] private GameObject enemyShip = null;
    [SerializeField] private GameObject[] asteroids = new GameObject[0];
    [SerializeField] private GameObject[] powerups = new GameObject[0];

    private AudioSource audioSource;
    private long score = 0;
    private long wave = 1;
    private bool loading = false;
    private long wavesCleared = 0; //Used for when the shields should be respawned
    private long wavesTillBoss = 0; //Only used in Survival Mode NIGHTMARE!
    private int clickSource = 1; //1 is pause menu, 2 is game over menu, 3 is level completed menu
    private bool showedNewHighScore = false;
    private bool playedLoseJingle = false, playedWinJingle = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        gameOver = false;
        won = false;
        paused = false;
        killsForLife = 0;
        Time.timeScale = 1;
        AudioListener.pause = false;
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
        if (canEnemyShipsSpawn) StartCoroutine(spawnEnemyShips());
        if (canAsteroidsSpawn) StartCoroutine(spawnAsteroids());
        StartCoroutine(spawnPowerups());
    }
    
    void Update()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        if (Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
        if (Input.GetKeyDown(KeyCode.Escape)) pause();
        if (!gameOver && !won && enemyHolder.transform.childCount <= 0)
        {
            if (isStandard)
            {
                if (wave < enemyPatterns.Length)
                {
                    ++wave;
                    ++wavesCleared;
                    spawnEnemyPattern();
                } else
                {
                    won = true;
                    if (!PlayerPrefs.HasKey("Wins"))
                    {
                        PlayerPrefs.SetString("Wins", "1");
                    } else
                    {
                        long plus = long.Parse(PlayerPrefs.GetString("Wins"));
                        ++plus;
                        PlayerPrefs.SetString("Wins", plus.ToString());
                    }
                    if (PlayerPrefs.GetInt("StandardLevel") >= PlayerPrefs.GetInt("MaxCampaignLevels"))
                    {
                        PlayerPrefs.SetInt("StandardLevel", 1);
                        levelCompletedUI.enabled = false;
                        StartCoroutine(loadScene("Campaign Ending"));
                    }
                    PlayerPrefs.Save();
                }
            } else
            {
                ++wave;
                ++wavesCleared;
                if (difficulty >= 4) ++wavesTillBoss;
                scoreMultiplier += 0.01f;
                spawnEnemyPattern();
            }
            if (!gameOver && !won)
            {
                if (!isStandard)
                {
                    if (difficulty <= 1)
                    {
                        enemyMoveTime -= 0.015f;
                        enemyFireRate -= 0.15f;
                        enemyBulletSpeedIncrement -= 0.0875f;
                    } else if (difficulty == 2)
                    {
                        enemyMoveTime -= 0.02f;
                        enemyFireRate -= 0.2f;
                        enemyBulletSpeedIncrement += 0.125f;
                    } else if (difficulty == 3)
                    {
                        enemyMoveTime -= 0.025f;
                        enemyFireRate -= 0.3f;
                        enemyBulletSpeedIncrement -= 0.15f;
                    } else if (difficulty >= 4)
                    {
                        enemyMoveTime -= 0.035f;
                        enemyFireRate -= 0.4f;
                        enemyBulletSpeedIncrement -= 0.2f;
                    }
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
            }
        } else
        {
            gameOverUI.enabled = false;
        }
        if (isStandard && !gameOver && won)
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
        if (isStandard)
        {
            levelCount.transform.parent.gameObject.SetActive(true);
            if (PlayerPrefs.HasKey("StandardLevel"))
            {
                levelCount.text = PlayerPrefs.GetInt("StandardLevel").ToString();
            } else
            {
                levelCount.text = "1";
            }
            scoreCount.transform.parent.gameObject.SetActive(false);
            waveCount.text = wave + "/" + enemyPatterns.Length;
        } else
        {
            levelCount.transform.parent.gameObject.SetActive(false);
            scoreCount.transform.parent.gameObject.SetActive(true);
            scoreCount.text = score.ToString();
            waveCount.text = wave.ToString();
        }
        if (!loading)
        {
            GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");
            loadingText.enabled = false;
            foreach (GameObject background in backgrounds)
            {
                background.transform.position = new Vector3(0, background.transform.position.y, 0);
                Camera.main.transform.position = new Vector3(0, 0, -10);
                gameUIMain.SetActive(true);
            }
        } else
        {
            GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("Background");
            loadingText.enabled = true;
            foreach (GameObject background in backgrounds)
            {
                background.transform.position = new Vector3(1000, background.transform.position.y, 0);
                Camera.main.transform.position = new Vector3(1000, 0, -10);
                gameUIMain.SetActive(false);
            }
        }
        if (enemyMoveTime < maxEnemyMoveTime) enemyMoveTime = maxEnemyMoveTime; //Checks if enemy move time is exceeding the maximum
        if (enemyFireRate < maxEnemyFireRate) enemyFireRate = maxEnemyFireRate; //Checks if enemy fire rate is exceeding the maximum
        if (enemyBulletSpeedIncrement > maxEnemyBulletSpeedIncrement) enemyBulletSpeedIncrement = maxEnemyBulletSpeedIncrement; //Checks if enemy bullet speed increment is exceeding the maximum
        if (isStandard) scoreMultiplier = 0; //Checks if the gamemode being played is Campaign
        if (!isStandard && difficulty < 4) //Checks if Survival Mode is being played on difficulties below NIGHTMARE!
        {
            NIGHTMAREBosses = new GameObject[0];
            killsForLife = 0;
        }
        if (scoreMultiplier < 0) scoreMultiplier = 0; //Checks if the score multiplier is below 0
    }

    void spawnEnemyPattern()
    {
        GameObject pattern;
        if (isStandard)
        {
            if (wave < enemyPatterns.Length + 1)
            {
                pattern = Instantiate(enemyPatterns[wave - 1], new Vector3(0, 19.5f, 0), enemyHolder.transform.rotation);
            } else
            {
                pattern = null;
            }
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
        if (pattern) pattern.transform.SetParent(enemyHolder.transform);
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
                    float random = Random.value;
                    if (random <= 0.5f)
                    {
                        ship = Instantiate(enemyShip, new Vector3(-18, 7.5f, 0), Quaternion.Euler(-90, 0, 0));
                        if (!isStandard && difficulty <= 1) ship.GetComponent<EnemyHealth>().health = 1;
                        ship.GetComponent<Mover>().speed = 5;
                    } else
                    {
                        ship = Instantiate(enemyShip, new Vector3(18, 7.5f, 0), Quaternion.Euler(-90, 0, 0));
                        if (!isStandard && difficulty <= 1) ship.GetComponent<EnemyHealth>().health = 1;
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
                yield return new WaitForSeconds(Random.Range(powerupSpawnTime.x, powerupSpawnTime.y));
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

    public void addScore(long value)
    {
        if (!isStandard && !gameOver && value > 0) score += (long)(value * scoreMultiplier);
    }

    void setNewHighScore(string key)
    {
        if (key != "" && !isStandard)
        {
            if (!PlayerPrefs.HasKey(key) && score > 0)
            {
                PlayerPrefs.SetString(key, score.ToString());
                if (!showedNewHighScore)
                {
                    showedNewHighScore = true;
                    highScoreIndicator.text = "NEW HIGH SCORE!";
                    Invoke("stopHighScoreIndicator", 3);
                }
            } else if (PlayerPrefs.HasKey(key) && score > long.Parse(PlayerPrefs.GetString(key)))
            {
                PlayerPrefs.SetString(key, long.Parse(PlayerPrefs.GetString(key)).ToString());
                if (!showedNewHighScore)
                {
                    showedNewHighScore = true;
                    highScoreIndicator.text = "NEW HIGH SCORE!";
                    Invoke("stopHighScoreIndicator", 3);
                }
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

    void pause()
    {
        if (!gameOver && !won && !settingsUI.enabled && !restartUI.enabled && !exitToMainMenuUI.enabled && !quitGameUI.enabled)
        {
            clickSource = 1;
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0;
                AudioListener.pause = true;
                gamePausedUI.enabled = true;
            } else
            {
                paused = false;
                Time.timeScale = 1;
                AudioListener.pause = false;
                gamePausedUI.enabled = false;
            }
        }
    }

    public void resumeGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        paused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        gamePausedUI.enabled = false;
    }

    public void toNextLevel()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading && isStandard)
        {
            loading = true;
            if (PlayerPrefs.GetInt("StandardLevel") < PlayerPrefs.GetInt("MaxCampaignLevels"))
            {
                PlayerPrefs.SetInt("StandardLevel", PlayerPrefs.GetInt("StandardLevel") + 1);
                StartCoroutine(loadScene("Level " + PlayerPrefs.GetInt("StandardLevel")));
            } else
            {
                PlayerPrefs.SetInt("StandardLevel", 1);
                StartCoroutine(loadScene("Campaign Ending"));
            }
            PlayerPrefs.Save();
        }
    }

    public void restartGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading)
        {
            loading = true;
            StartCoroutine(loadScene(SceneManager.GetActiveScene().name));
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

    public void exitToMainMenu()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading)
        {
            loading = true;
            StartCoroutine(loadScene("Main Menu"));
        }
    }

    public void exitGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        Application.Quit();
    }

    IEnumerator loadScene(string scene)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);
        while (!load.isDone)
        {
            loadingText.text = "Loading: " + Mathf.Floor(load.progress * 100) + "%";
            gamePausedUI.enabled = false;
            gameOverUI.enabled = false;
            levelCompletedUI.enabled = false;
            restartUI.enabled = false;
            exitToMainMenuUI.enabled = false;
            quitGameUI.enabled = false;
            yield return null;
        }
        loading = false;
        gameUIMain.SetActive(true);
    }
}