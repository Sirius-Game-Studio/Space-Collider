using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("High Scores Menu")]
    [SerializeField] private Text endlessEasyHighScore = null;
    [SerializeField] private Text endlessNormalHighScore = null;
    [SerializeField] private Text endlessHardHighScore = null;
    [SerializeField] private Text endlessNIGHTMAREHighScore = null;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip buttonClick = null;

    [Header("Setup")]
    [SerializeField] private Canvas mainMenu = null;
    [SerializeField] private Canvas highScoresMenu = null;
    [SerializeField] private Canvas settingsMenu = null;
    [SerializeField] private Canvas gamemodesMenu = null;
    [SerializeField] private Canvas selectDifficultyMenu = null;
    [SerializeField] private Canvas clearHighScoresPrompt = null;
    [SerializeField] private Text loadingText = null;
    [SerializeField] private AudioMixer audioMixer = null;

    private AudioSource audioSource;
    private bool loading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource) audioSource.ignoreListenerPause = true;
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
        mainMenu.enabled = true;
        highScoresMenu.enabled = false;
        settingsMenu.enabled = false;
        gamemodesMenu.enabled = false;
        selectDifficultyMenu.enabled = false;
        clearHighScoresPrompt.enabled = false;
    }
    
    void Update()
    {
        updateHighScore(endlessEasyHighScore, "EasyHighScore", "Easy");
        updateHighScore(endlessNormalHighScore, "NormalHighScore", "Normal");
        updateHighScore(endlessHardHighScore, "HardHighScore", "Hard");
        updateHighScore(endlessNIGHTMAREHighScore, "NightmareHighScore", "NIGHTMARE!");
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
        if (!gamemodesMenu.enabled)
        {
            mainMenu.enabled = false;
            gamemodesMenu.enabled = true;
        } else
        {
            mainMenu.enabled = true;
            gamemodesMenu.enabled = false;
        }
    }

    public void clickHighScores()
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
        if (!highScoresMenu.enabled)
        {
            mainMenu.enabled = false;
            highScoresMenu.enabled = true;
        } else
        {
            mainMenu.enabled = true;
            highScoresMenu.enabled = false;
        }
    }

    public void clickSettings()
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
        if (!settingsMenu.enabled)
        {
            mainMenu.enabled = false;
            settingsMenu.enabled = true;
        } else
        {
            mainMenu.enabled = true;
            settingsMenu.enabled = false;
        }
    }

    public void clickSurvivalMode()
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
        if (!selectDifficultyMenu.enabled)
        {
            gamemodesMenu.enabled = false;
            selectDifficultyMenu.enabled = true;
        } else
        {
            gamemodesMenu.enabled = true;
            selectDifficultyMenu.enabled = false;
        }
    }

    
    public void clickClearHighScore()
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
        if (!clearHighScoresPrompt.enabled)
        {
            highScoresMenu.enabled = false;
            clearHighScoresPrompt.enabled = true;
        } else
        {
            highScoresMenu.enabled = true;
            clearHighScoresPrompt.enabled = false;
        }
    }

    public void clickQuitGame()
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

    public void startCampaign()
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
        if (PlayerPrefs.GetInt("StandardLevel") > 0)
        {
            StartCoroutine(loadScene("Level " + PlayerPrefs.GetInt("StandardLevel")));
        } else
        {
            PlayerPrefs.SetInt("StandardLevel", 1);
            PlayerPrefs.Save();
            StartCoroutine(loadScene("Level 1"));
        }
    }

    public void startSurvival(string difficulty)
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
        if (difficulty != "")
        {
            StartCoroutine(loadScene("Survival " + difficulty));
        } else
        {
            StartCoroutine(loadScene("Survival Normal"));
        }
    }


    public void clearHighScores()
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
        PlayerPrefs.DeleteKey("EasyHighScore");
        PlayerPrefs.DeleteKey("NormalHighScore");
        PlayerPrefs.DeleteKey("HardHighScore");
        PlayerPrefs.DeleteKey("NightmareHighScore");
        PlayerPrefs.Save();
        clearHighScoresPrompt.enabled = false;
        highScoresMenu.enabled = true;
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
                mainMenu.enabled = false;
                highScoresMenu.enabled = false;
                settingsMenu.enabled = false;
                gamemodesMenu.enabled = false;
                selectDifficultyMenu.enabled = false;
                clearHighScoresPrompt.enabled = false;
                yield return null;
            }
        }
    }
}