using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Canvas endingUI;
    [SerializeField] private Canvas creditsUI;
    [SerializeField] private Animator creditsAnimator;
    [SerializeField] private Text loadingText;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;
    private LevelHolder levelHolder;
    private bool loading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        levelHolder = FindObjectOfType<LevelHolder>();
        if (levelHolder) levelHolder.level = 1;
        PlayerPrefs.SetInt("StandardLevel", 1);
        PlayerPrefs.Save();
        if (audioSource) audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        endingUI.enabled = true;
        creditsUI.enabled = false;
    }

    void Update()
    {
        if (audioSource) audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        if (Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
        if (!loading)
        {
            loadingText.enabled = false;
        } else
        {
            loadingText.enabled = true;
        }
    }

    public void toMainMenu()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!loading)
        {
            loading = true;
            endingUI.enabled = false;
            StartCoroutine(loadScene("Main Menu"));
        }
    }

    public void clickQuitGame()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        Application.Quit();
    }

    public void clickCredits()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!creditsUI.enabled)
        {
            endingUI.enabled = false;
            creditsUI.enabled = true;
            creditsAnimator.SetBool("CreditsOpen", true);
        } else
        {
            endingUI.enabled = true;
            creditsUI.enabled = false;
            creditsAnimator.SetBool("CreditsOpen", false);
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
        endingUI.enabled = true;
    }
}
