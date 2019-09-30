using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Settings")]
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

    [Header("Survival-Only Settings")]
    [SerializeField] private float scoreMultiplier = 1;
    [Tooltip("1 is Easy, 2 is Normal, 3 is Hard, 4 is NIGHTMARE!.")] [Range(1, 4)] public int difficulty = 1;
    [Tooltip("NIGHTMARE! only.")] [SerializeField] private GameObject[] NIGHTMAREBosses = new GameObject[0];

    [Header("Sound Effects")]
    [SerializeField] private AudioClip buttonClick = null;
    [SerializeField] private AudioClip gameOverJingle = null;
    [SerializeField] private AudioClip winJingle = null;

    [Header("UI")]
    [SerializeField] private Canvas gameHUD = null;
    [SerializeField] private Canvas gamePausedMenu = null;
    [SerializeField] private Canvas gameOverMenu = null;
    [SerializeField] private Canvas levelCompletedMenu = null;
    [SerializeField] private Canvas settingsMenu = null;
    [SerializeField] private Canvas quitGameMenu = null;
    [SerializeField] private Canvas restartPrompt = null;
    [SerializeField] private Text levelCount = null;
    [SerializeField] private Text scoreCount = null;
    [SerializeField] private Text waveCount = null;
    [SerializeField] private Text highScoreIndicator = null;
    [SerializeField] private Text message = null;
    [SerializeField] private Text loadingText = null;

    [Header("Miscellaneous")]
    public bool isCampaignLevel = true;
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
    [SerializeField] private AudioMixer audioMixer = null;

    private AudioSource audioSource;
    private Controls input;
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
        input = new Controls();
        if (audioSource) audioSource.ignoreListenerPause = true;
        loading = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 1);
            PlayerPrefs.Save();
        } else
        {
            audioMixer.SetFloat("SoundVolume", Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume")) * 20);
        }
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.Save();
        } else
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }
        gameOver = false;
        won = false;
        paused = false;
        killsForLife = 0;
        gameHUD.enabled = true;
        gamePausedMenu.enabled = false;
        gameOverMenu.enabled = false;
        levelCompletedMenu.enabled = false;
        settingsMenu.enabled = false;
        quitGameMenu.enabled = false;
        restartPrompt.enabled = false;
        spawnEnemyPattern();
        if (canEnemyShipsSpawn) StartCoroutine(spawnEnemyShips());
        if (canAsteroidsSpawn) StartCoroutine(spawnAsteroids());
        StartCoroutine(spawnPowerups());
    }

    void OnEnable()
    {
        input.Enable();
        input.Gameplay.Fullscreen.performed += context => toggleFullscreen();
        input.Gameplay.Pause.performed += context => pause();
        input.Gameplay.Resume.performed += context => resumeGame(false);
        input.Gameplay.Restart.performed += context => restart(false);
        input.Menu.CloseMenu.performed += context => closeMenu();
    }

    void OnDisable()
    {
        input.Disable();
        input.Gameplay.Fullscreen.performed -= context => toggleFullscreen();
        input.Gameplay.Pause.performed -= context => pause();
        input.Gameplay.Resume.performed -= context => resumeGame(false);
        input.Gameplay.Restart.performed -= context => restart(false);
        input.Menu.CloseMenu.performed -= context => closeMenu();
    }
    
    void Update()
    {
        if (!gameOver && !won && enemyHolder.transform.childCount <= 0)
        {
            if (isCampaignLevel)
            {
                if (wave < enemyPatterns.Length)
                {
                    ++wave;
                    ++wavesCleared;
                    spawnEnemyPattern();
                } else
                {
                    won = true;
                    if (PlayerPrefs.GetInt("StandardLevel") >= PlayerPrefs.GetInt("MaxCampaignLevels"))
                    {
                        PlayerPrefs.SetInt("StandardLevel", 1);
                        levelCompletedMenu.enabled = false;
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
            if (!gameOver && !won && !isCampaignLevel)
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
            if (!quitGameMenu.enabled && !restartPrompt.enabled) gameOverMenu.enabled = true;
            clickSource = 2;
            if (audioSource && gameOverJingle && !playedLoseJingle)
            {
                playedLoseJingle = true;
                audioSource.PlayOneShot(gameOverJingle);
            }
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            if (!isCampaignLevel)
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
            gameOverMenu.enabled = false;
        }
        if (isCampaignLevel && !gameOver && won)
        {
            if (!loading && !quitGameMenu.enabled && !restartPrompt.enabled) levelCompletedMenu.enabled = true;
            clickSource = 3;
            if (PlayerPrefs.GetInt("StandardLevel") < PlayerPrefs.GetInt("MaxLevels"))
            {
                if (audioSource && winJingle && !playedWinJingle)
                {
                    playedWinJingle = true;
                    audioSource.PlayOneShot(winJingle);
                }
            } else
            {
                StartCoroutine(loadScene("Ending"));
            }
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
        }
        if (isCampaignLevel)
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
            Camera.main.transform.position = new Vector3(0, 0, -10);
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player) player.SetActive(true);
            }
            foreach (GameObject background in GameObject.FindGameObjectsWithTag("Background"))
            {
                if (background)
                {
                    background.transform.position = new Vector3(0, background.transform.position.y, 5);
                    background.GetComponent<BackgroundScroll>().enabled = true;
                }
            }
        } else
        {
            Camera.main.transform.position = new Vector3(500, 0, -10);
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player) player.SetActive(false);
            }
            foreach (GameObject background in GameObject.FindGameObjectsWithTag("Background"))
            {
                if (background)
                {
                    background.transform.position = new Vector3(500, background.transform.position.y, 5);
                    background.GetComponent<BackgroundScroll>().enabled = false;
                }
            }
        }
        if (enemyMoveTime < maxEnemyMoveTime) enemyMoveTime = maxEnemyMoveTime; //Checks if enemy move time is exceeding the maximum
        if (enemyFireRate < maxEnemyFireRate) enemyFireRate = maxEnemyFireRate; //Checks if enemy fire rate is exceeding the maximum
        if (enemyBulletSpeedIncrement > maxEnemyBulletSpeedIncrement) enemyBulletSpeedIncrement = maxEnemyBulletSpeedIncrement; //Checks if enemy bullet speed increment is exceeding the maximum
        if (isCampaignLevel) scoreMultiplier = 0; //Checks if the gamemode being played is Campaign
        if (!isCampaignLevel && difficulty < 4) //Checks if Survival Mode is being played on difficulties below NIGHTMARE!
        {
            NIGHTMAREBosses = new GameObject[0];
            killsForLife = 0;
        }
        if (scoreMultiplier < 0) scoreMultiplier = 0; //Checks if the score multiplier is below 0
    }

    void toggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    void closeMenu()
    {
        if (paused)
        {
            if (settingsMenu.enabled)
            {
                settingsMenu.enabled = false;
                if (clickSource <= 1)
                {
                    gamePausedMenu.enabled = true;
                } else if (clickSource == 2)
                {
                    gameOverMenu.enabled = true;
                } else if (clickSource >= 3)
                {
                    levelCompletedMenu.enabled = true;
                }
            } else if (quitGameMenu.enabled)
            {
                quitGameMenu.enabled = false;
                if (clickSource <= 1)
                {
                    gamePausedMenu.enabled = true;
                } else if (clickSource == 2)
                {
                    gameOverMenu.enabled = true;
                } else if (clickSource >= 3)
                {
                    levelCompletedMenu.enabled = true;
                }
            } else if (restartPrompt.enabled)
            {
                restartPrompt.enabled = false;
                if (clickSource <= 1)
                {
                    gamePausedMenu.enabled = true;
                } else if (clickSource == 2)
                {
                    gameOverMenu.enabled = true;
                } else if (clickSource >= 3)
                {
                    levelCompletedMenu.enabled = true;
                }
            }
        }
    }

    void spawnEnemyPattern()
    {
        GameObject pattern;
        if (isCampaignLevel)
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
                    if (random <= 0.5f) //Left
                    {
                        ship = Instantiate(enemyShip, new Vector3(-18, 7.5f, 0), Quaternion.Euler(-90, 0, 0));
                        if (!isCampaignLevel && difficulty <= 1) ship.GetComponent<EnemyHealth>().health = 1;
                        ship.GetComponent<Mover>().speed = 5;
                    } else //Right
                    {
                        ship = Instantiate(enemyShip, new Vector3(18, 7.5f, 0), Quaternion.Euler(-90, 0, 0));
                        if (!isCampaignLevel && difficulty <= 1) ship.GetComponent<EnemyHealth>().health = 1;
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
        if (!isCampaignLevel && !gameOver && value > 0) score += (long)(value * scoreMultiplier);
    }

    void setNewHighScore(string key)
    {
        if (key != "" && !isCampaignLevel)
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
        if (!gameOver && !won && !gameOverMenu.enabled && !levelCompletedMenu.enabled)
        {
            if (!paused) //Pauses the game
            {
                clickSource = 1;
                paused = true;
                Time.timeScale = 0;
                AudioListener.pause = true;
                gamePausedMenu.enabled = true;
            } else //Unpauses the game
            {
                if (!settingsMenu.enabled && !quitGameMenu.enabled && !restartPrompt.enabled)
                {
                    paused = false;
                    Time.timeScale = 1;
                    AudioListener.pause = false;
                    gamePausedMenu.enabled = false;
                }
            }
        }
    }

    public void resumeGame(bool wasClicked)
    {
        if (!settingsMenu.enabled && !quitGameMenu.enabled && !restartPrompt.enabled)
        {
            if (audioSource && wasClicked)
            {
                if (buttonClick)
                {
                    audioSource.PlayOneShot(buttonClick);
                } else
                {
                    audioSource.Play();
                }
            }
            paused = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
            gamePausedMenu.enabled = false;
        }
    }

    public void toNextLevel()
    {
        if (isCampaignLevel && won && levelCompletedMenu.enabled)
        {
            if (audioSource)
            {
                if (buttonClick)
                {
                    audioSource.PlayOneShot(buttonClick);
                } else
                {
                    audioSource.Play();
                }
            }
            if (PlayerPrefs.GetInt("StandardLevel") < PlayerPrefs.GetInt("MaxCampaignLevels"))
            {
                PlayerPrefs.SetInt("StandardLevel", PlayerPrefs.GetInt("StandardLevel") + 1);
                StartCoroutine(loadScene("Level " + PlayerPrefs.GetInt("StandardLevel")));
            } else
            {
                PlayerPrefs.SetInt("StandardLevel", 1);
                StartCoroutine(loadScene("Ending"));
            }
            PlayerPrefs.Save();
        }
    }

    public void restart(bool wasClicked)
    {
        if (gameOverMenu.enabled || restartPrompt.enabled)
        {
            if (audioSource && wasClicked)
            {
                if (buttonClick)
                {
                    audioSource.PlayOneShot(buttonClick);
                } else
                {
                    audioSource.Play();
                }
            }
            StartCoroutine(loadScene(SceneManager.GetActiveScene().name));
        }
    }

    public void exitGame()
    {
        if (audioSource)
        {
            if (buttonClick)
            {
                audioSource.PlayOneShot(buttonClick);
            } else
            {
                audioSource.Play();
            }
        }
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void exitToMainMenu()
    {
        if (audioSource)
        {
            if (buttonClick)
            {
                audioSource.PlayOneShot(buttonClick);
            } else
            {
                audioSource.Play();
            }
        }
        StartCoroutine(loadScene("Main Menu"));
    }

    public void openCanvasFromClickSource(Canvas canvas)
    {
        if (canvas)
        {
            if (audioSource)
            {
                if (buttonClick)
                {
                    audioSource.PlayOneShot(buttonClick);
                } else
                {
                    audioSource.Play();
                }
            }
            if (!canvas.enabled)
            {
                canvas.enabled = true;
                if (clickSource <= 1)
                {
                    gamePausedMenu.enabled = false;
                } else if (clickSource == 2)
                {
                    gameOverMenu.enabled = false;
                } else if (clickSource >= 3)
                {
                    levelCompletedMenu.enabled = false;
                }
            } else
            {
                canvas.enabled = false;
                if (clickSource <= 1)
                {
                    gamePausedMenu.enabled = true;
                } else if (clickSource == 2)
                {
                    gameOverMenu.enabled = true;
                } else if (clickSource >= 3)
                {
                    levelCompletedMenu.enabled = true;
                }
            }
        }
    }

    IEnumerator loadScene(string scene)
    {
        if (!loading)
        {
            loading = true;
            AsyncOperation load = SceneManager.LoadSceneAsync(scene);
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            while (!load.isDone)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                loadingText.text = "Loading: " + Mathf.Floor(load.progress * 100) + "%";
                gameHUD.enabled = false;
                gamePausedMenu.enabled = false;
                gameOverMenu.enabled = false;
                levelCompletedMenu.enabled = false;
                settingsMenu.enabled = false;
                quitGameMenu.enabled = false;
                restartPrompt.enabled = false;
                yield return null;
            }
        }
    }
}