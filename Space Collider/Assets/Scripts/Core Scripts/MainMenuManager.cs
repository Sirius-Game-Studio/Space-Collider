using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Records Menu")]
    [SerializeField] private Canvas recordsUI;
    [SerializeField] private Canvas standardRecordsUI;
    [SerializeField] private Canvas endlessRecordsUI;
    [SerializeField] private Canvas resetProgressPrompt;
    [SerializeField] private Canvas clearHighScorePrompt;
    [SerializeField] private Text aliensKilled;
    [SerializeField] private Text asteroidsDestroyed;
    [SerializeField] private Text deaths;
    [SerializeField] private Text standardProgress;
    [SerializeField] private Text standardWins;
    [SerializeField] private Text standardLoses;
    [SerializeField] private Text endlessEasyHighScore;
    [SerializeField] private Text endlessNormalHighScore;
    [SerializeField] private Text endlessHardHighScore;
    [SerializeField] private Text endlessNIGHTMAREHighScore;

    [Header("Settings Menu")]
    [SerializeField] private Canvas settingsUI;
    [SerializeField] private Text fullscreenText;

    [Header("Sound Menu")]
    [SerializeField] private Canvas soundUI;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Setup")]
    [SerializeField] private Canvas mainMenuUI;
    [SerializeField] private Canvas gamemodesUI;
    [SerializeField] private Canvas graphicsQualityUI;
    [SerializeField] private Canvas selectDifficultyUI;
    [SerializeField] private Text loadingText;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;
    private LevelHolder levelHolder;
    private bool loading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        levelHolder = FindObjectOfType<LevelHolder>();
        if (!PlayerPrefs.HasKey("StandardLevel"))
        {
            PlayerPrefs.SetInt("StandardLevel", 1);
            PlayerPrefs.Save();
        }
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
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        mainMenuUI.enabled = true;
        gamemodesUI.enabled = false;
        recordsUI.enabled = false;
        settingsUI.enabled = false;
        graphicsQualityUI.enabled = false;
        soundUI.enabled = false;
        selectDifficultyUI.enabled = false;
        standardRecordsUI.enabled = false;
        endlessRecordsUI.enabled = false;
        resetProgressPrompt.enabled = false;
        clearHighScorePrompt.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        aliensKilled.text = "Aliens Killed: " + PlayerPrefs.GetInt("AliensKilled");
        asteroidsDestroyed.text = "Asteroids Destroyed: " + PlayerPrefs.GetInt("AsteroidsDestroyed");
        deaths.text = "Deaths: " + PlayerPrefs.GetInt("Deaths");
        if (PlayerPrefs.HasKey("StandardLevel"))
        {
            standardProgress.text = "Level Progress: " + PlayerPrefs.GetInt("StandardLevel") + "/11";
        } else if (!PlayerPrefs.HasKey("StandardLevel") && levelHolder)
        {
            standardProgress.text = "Level Progress: " + levelHolder.level + "/11";
        } else if (!PlayerPrefs.HasKey("StandardLevel") && !levelHolder)
        {
            standardProgress.text = "Level Progress: 1/11";
        }
        standardWins.text = "Wins: " + PlayerPrefs.GetInt("Wins");
        standardLoses.text = "Loses: " + PlayerPrefs.GetInt("Loses");
        endlessEasyHighScore.text = "High Score on Easy: " + PlayerPrefs.GetInt("EasyHighScore");
        endlessNormalHighScore.text = "High Score on Normal: " + PlayerPrefs.GetInt("NormalHighScore");
        endlessHardHighScore.text = "High Score on Hard: " + PlayerPrefs.GetInt("HardHighScore");
        endlessNIGHTMAREHighScore.text = "High Score on NIGHTMARE!: " + PlayerPrefs.GetInt("NightmareHighScore");
        if (Screen.fullScreen)
        {
            fullscreenText.text = "Change to Windowed Mode";
            fullscreenText.rectTransform.sizeDelta = new Vector2(294, 23);
        } else
        {
            fullscreenText.text = "Change to Fullscreen";
            fullscreenText.rectTransform.sizeDelta = new Vector2(223, 23);
        }
        if (!loading)
        {
            loadingText.enabled = false;
        } else
        {
            loadingText.enabled = true;
        }
    }
    
    public void clickPlay()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!gamemodesUI.enabled)
        {
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = true;
        } else
        {
            mainMenuUI.enabled = true;
            gamemodesUI.enabled = false;
        }
    }

    public void clickRecords()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!recordsUI.enabled)
        {
            mainMenuUI.enabled = false;
            recordsUI.enabled = true;
        } else
        {
            mainMenuUI.enabled = true;
            recordsUI.enabled = false;
        }
    }

    public void clickSettings()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!settingsUI.enabled)
        {
            mainMenuUI.enabled = false;
            settingsUI.enabled = true;
        } else
        {
            mainMenuUI.enabled = true;
            settingsUI.enabled = false;
        }
    }

    public void clickPlayEndlessMode()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!selectDifficultyUI.enabled)
        {
            gamemodesUI.enabled = false;
            selectDifficultyUI.enabled = true;
        } else
        {
            gamemodesUI.enabled = true;
            selectDifficultyUI.enabled = false;
        }
    }

    public void clickStandardModeRecords()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!standardRecordsUI.enabled)
        {
            recordsUI.enabled = false;
            standardRecordsUI.enabled = true;
        } else
        {
            recordsUI.enabled = true;
            standardRecordsUI.enabled = false;
        }
    }

    public void clickEndlessModeRecords()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!endlessRecordsUI.enabled)
        {
            recordsUI.enabled = false;
            endlessRecordsUI.enabled = true;
        } else
        {
            recordsUI.enabled = true;
            endlessRecordsUI.enabled = false;
        }
    }

    public void clickResetProgress()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!resetProgressPrompt.enabled)
        {
            standardRecordsUI.enabled = false;
            resetProgressPrompt.enabled = true;
        } else
        {
            standardRecordsUI.enabled = true;
            resetProgressPrompt.enabled = false;
        }
    }

    public void clickClearHighScore()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!clearHighScorePrompt.enabled)
        {
            endlessRecordsUI.enabled = false;
            clearHighScorePrompt.enabled = true;
        }
        else
        {
            endlessRecordsUI.enabled = true;
            clearHighScorePrompt.enabled = false;
        }
    }

    public void clickGraphicsQuality()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!graphicsQualityUI.enabled)
        {
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = true;
        } else
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
        } else
        {
            settingsUI.enabled = true;
            soundUI.enabled = false;
        }
    }

    public void clickQuitGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        Application.Quit();
    }

    public void selectStandardMode()
    {
        if (gamemodesUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = false;
            soundUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (PlayerPrefs.HasKey("StandardLevel"))
            {
                StartCoroutine(loadScene("Level " + PlayerPrefs.GetInt("StandardLevel")));
            } else if (!PlayerPrefs.HasKey("StandardLevel") && levelHolder)
            {
                StartCoroutine(loadScene("Level " + levelHolder.level));
            } else if (!PlayerPrefs.HasKey("StandardLevel") && !levelHolder)
            {
                PlayerPrefs.SetInt("StandardLevel", 1);
                PlayerPrefs.Save();
                StartCoroutine(loadScene("Level 1"));
            }
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
        }
    }

    public void selectEndlessModeEasy()
    {
        if (selectDifficultyUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = false;
            soundUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            StartCoroutine(loadScene("Endless Mode Easy"));
        }
    }

    public void selectEndlessModeNormal()
    {
        if (selectDifficultyUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = false;
            soundUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            StartCoroutine(loadScene("Endless Mode Normal"));
        }
    }

    public void selectEndlessModeHard()
    {
        if (selectDifficultyUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = false;
            soundUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            StartCoroutine(loadScene("Endless Mode Hard"));
        }
    }

    public void selectEndlessModeNightmare()
    {
        if (selectDifficultyUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            graphicsQualityUI.enabled = false;
            soundUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            StartCoroutine(loadScene("Endless Mode NIGHTMARE!"));
        }
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

    public void resetProgress()
    {
        if (resetProgressPrompt.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            if (levelHolder) levelHolder.level = 1;
            PlayerPrefs.SetInt("StandardLevel", 1);
            PlayerPrefs.Save();
            resetProgressPrompt.enabled = false;
            standardRecordsUI.enabled = true;
            print("Standard Mode progress has been cleared.");
        }
    }

    public void clearHighScore()
    {
        if (clearHighScorePrompt.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            PlayerPrefs.DeleteKey("EasyHighScore");
            PlayerPrefs.DeleteKey("NormalHighScore");
            PlayerPrefs.DeleteKey("HardHighScore");
            PlayerPrefs.Save();
            clearHighScorePrompt.enabled = false;
            endlessRecordsUI.enabled = true;
            print("Endless Mode high scores have been cleared.");
        }
    }

    IEnumerator loadScene(string scene)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene);
        while (!load.isDone)
        {
            loadingText.text = "Loading: " + Mathf.Floor(load.progress * 100) + "%";
            yield return null;
        }
        loading = false;
        mainMenuUI.enabled = true;
    }
}