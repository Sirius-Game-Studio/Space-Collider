using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingUIController : MonoBehaviour
{
    [SerializeField] private Text loadingText;
    [SerializeField] private Text versionText;
    [SerializeField] private Canvas endingUI;
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
    }

    void Update()
    {
        if (audioSource) audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        if (Camera.main.GetComponent<AudioSource>()) Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        if (Input.GetKeyDown(KeyCode.F11)) Screen.fullScreen = !Screen.fullScreen;
        if (versionText) versionText.text = "Version: " + Application.version;
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
