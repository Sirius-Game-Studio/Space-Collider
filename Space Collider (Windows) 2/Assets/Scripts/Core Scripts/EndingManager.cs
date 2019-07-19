using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Canvas endingUI;
    [SerializeField] private Canvas creditsUI;
    [SerializeField] private RectTransform credits;
    [SerializeField] private float creditsPositionY = 400;
    [SerializeField] private float creditsScrollSpeed = 1;
    [SerializeField] private Text loadingText;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;
    private bool loading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (!creditsUI.enabled) credits.anchoredPosition = new Vector2(0, creditsPositionY);
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

    public void clickCredits()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound, PlayerPrefs.GetFloat("SoundVolume"));
        if (!creditsUI.enabled)
        {
            endingUI.enabled = false;
            creditsUI.enabled = true;
            StartCoroutine(scrollCredits());
        } else
        {
            endingUI.enabled = true;
            creditsUI.enabled = false;
            StopCoroutine(scrollCredits());
        }
    }

    IEnumerator scrollCredits()
    {
        while (creditsUI.enabled)
        {
            yield return new WaitForEndOfFrame();
            if (creditsUI.enabled) credits.anchoredPosition -= new Vector2(0, creditsScrollSpeed);
            if (credits.anchoredPosition.y <= -creditsPositionY)
            {
                endingUI.enabled = true;
                creditsUI.enabled = false;
                StopCoroutine(scrollCredits());
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
        endingUI.enabled = true;
    }
}