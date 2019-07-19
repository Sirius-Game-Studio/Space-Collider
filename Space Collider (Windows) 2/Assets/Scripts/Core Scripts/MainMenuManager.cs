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
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Text fullscreenText;

    [Header("Setup")]
    [SerializeField] private Canvas mainMenuUI;
    [SerializeField] private Canvas gamemodesUI;
    [SerializeField] private Canvas selectDifficultyUI;
    [SerializeField] private Text loadingText;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;
    private bool loading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        updateRecord(aliensKilled, "AliensKilled", "Aliens Killed");
        updateRecord(asteroidsDestroyed, "AsteroidsDestroyed", "Asteroids Destroyed");
        updateRecord(deaths, "Deaths", "Deaths");
        if (PlayerPrefs.HasKey("StandardLevel"))
        {
            standardProgress.text = "Current Level: " + PlayerPrefs.GetInt("StandardLevel");
        } else
        {
            standardProgress.text = "Current Level: 1";
        }
        updateRecord(standardWins, "Wins", "Wins");
        updateRecord(standardLoses, "Loses", "Loses");
        updateHighScore(endlessEasyHighScore, "EasyHighScore", "Easy");
        updateHighScore(endlessNormalHighScore, "NormalHighScore", "Normal");
        updateHighScore(endlessHardHighScore, "HardHighScore", "Hard");
        updateHighScore(endlessNIGHTMAREHighScore, "NightmareHighScore", "NIGHTMARE!");
        if (Screen.fullScreen)
        {
            fullscreenText.text = "Change to Windowed Mode";
            fullscreenText.rectTransform.sizeDelta = new Vector2(297, 24);
        } else
        {
            fullscreenText.text = "Change to Fullscreen";
            fullscreenText.rectTransform.sizeDelta = new Vector2(224, 24);
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

    public void clickSurvivalMode()
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

    public void clickCampaignRecords()
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

    public void clickSurvivalModeRecords()
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

    public void clickQuitGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        Application.Quit();
    }

    public void selectCampaign()
    {
        if (gamemodesUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (PlayerPrefs.HasKey("StandardLevel"))
            {
                StartCoroutine(loadScene("Level " + PlayerPrefs.GetInt("StandardLevel")));
            } else
            {
                PlayerPrefs.SetInt("StandardLevel", 1);
                PlayerPrefs.Save();
                StartCoroutine(loadScene("Level 1"));
            }
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
        }
    }

    public void selectSurvivalMode(string difficulty)
    {
        if (difficulty != "" && selectDifficultyUI.enabled && !loading)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            loading = true;
            mainMenuUI.enabled = false;
            gamemodesUI.enabled = false;
            recordsUI.enabled = false;
            settingsUI.enabled = false;
            selectDifficultyUI.enabled = false;
            standardRecordsUI.enabled = false;
            endlessRecordsUI.enabled = false;
            resetProgressPrompt.enabled = false;
            clearHighScorePrompt.enabled = false;
            if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().Stop();
            StartCoroutine(loadScene("Survival Mode " + difficulty));
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

    public void resetProgress()
    {
        if (resetProgressPrompt.enabled)
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
            PlayerPrefs.SetInt("StandardLevel", 1);
            PlayerPrefs.Save();
            resetProgressPrompt.enabled = false;
            standardRecordsUI.enabled = true;
            print("Campaign progress has been cleared.");
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
            PlayerPrefs.DeleteKey("NightmareHighScore");
            PlayerPrefs.Save();
            clearHighScorePrompt.enabled = false;
            endlessRecordsUI.enabled = true;
            print("Survival Mode high scores have been cleared.");
        }
    }

    void updateRecord(Text main, string key, string text)
    {
        if (main && key != "")
        {
            if (PlayerPrefs.HasKey(key))
            {
                main.text = text + ": " + PlayerPrefs.GetString(key);
            } else
            {
                main.text = text + ": 0";
            }
        }
    }

    void updateHighScore(Text main, string key, string difficulty)
    {
        if (main && key != "" && difficulty != "")
        {
            if (PlayerPrefs.HasKey(key))
            {
                main.text = "High Score on " + difficulty + ": " + PlayerPrefs.GetString(key);
            } else
            {
                main.text = "High Score on " + difficulty + ": 0";
            }
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