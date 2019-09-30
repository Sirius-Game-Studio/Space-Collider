using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [Header("Credits Settings")]
    [Tooltip("The Y position credits start at.")] [SerializeField] private float creditsY = 600;
    [SerializeField] private float creditsScrollSpeed = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip buttonClick = null;

    [Header("Setup")]
    [SerializeField] private Canvas endingMenu = null;
    [SerializeField] private Canvas creditsMenu = null;
    [SerializeField] private RectTransform credits = null;
    [SerializeField] private Text loadingText = null;
    [SerializeField] private AudioMixer audioMixer = null;

    private AudioSource audioSource;
    private Controls input;
    private bool loading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        input = new Controls();
        if (audioSource) audioSource.ignoreListenerPause = true;
        loading = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 1);
        } else
        {
            audioMixer.SetFloat("SoundVolume", Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume")) * 20);
        }
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        } else
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }
        PlayerPrefs.SetInt("StandardLevel", 1);
        PlayerPrefs.Save();
        endingMenu.enabled = true;
        creditsMenu.enabled = false;
    }

    void OnEnable()
    {
        input.Enable();
        input.Gameplay.Fullscreen.performed += context => toggleFullscreen();
        input.Menu.CloseMenu.performed += context => stopCredits();
    }

    void OnDisable()
    {
        input.Disable();
        input.Gameplay.Fullscreen.performed -= context => toggleFullscreen();
        input.Menu.CloseMenu.performed -= context => stopCredits();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            creditsMenu.enabled = false;
            endingMenu.enabled = true;
        }
        if (!creditsMenu.enabled) credits.anchoredPosition = new Vector2(0, creditsY);
        if (!loading)
        {
            loadingText.enabled = false;
        } else
        {
            loadingText.enabled = true;
        }
    }

    void toggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    void stopCredits()
    {
        if (creditsMenu.enabled)
        {
            creditsMenu.enabled = false;
            endingMenu.enabled = true;
            StopCoroutine(scrollCredits());
        }
    }

    public void clickCredits()
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
        if (!creditsMenu.enabled)
        {
            endingMenu.enabled = false;
            creditsMenu.enabled = true;
            StartCoroutine(scrollCredits());
        } else
        {
            endingMenu.enabled = true;
            creditsMenu.enabled = false;
            StopCoroutine(scrollCredits());
        }
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

    IEnumerator scrollCredits()
    {
        while (creditsMenu.enabled)
        {
            yield return new WaitForEndOfFrame();
            if (creditsMenu.enabled) credits.anchoredPosition -= new Vector2(0, creditsScrollSpeed);
            if (credits.anchoredPosition.y <= -creditsY)
            {
                endingMenu.enabled = true;
                creditsMenu.enabled = false;
                yield break;
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
                endingMenu.enabled = false;
                creditsMenu.enabled = false;
                yield return null;
            }
        }
    }
}