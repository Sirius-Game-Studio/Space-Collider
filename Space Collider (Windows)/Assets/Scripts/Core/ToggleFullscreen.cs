using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    [SerializeField] private Vector2 fullscreenTextSize = new Vector2(262, 30);
    [SerializeField] private Vector2 windowedModeTextSize = new Vector2(345, 30);
    [SerializeField] private AudioClip buttonClick = null;

    private Text fullscreenText;
    private AudioSource audioSource;

    void Start()
    {
        fullscreenText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource) audioSource.ignoreListenerPause = true;
    }

    void Update()
    {
        if (!Screen.fullScreen)
        {
            fullscreenText.text = "Change to Fullscreen";
            fullscreenText.rectTransform.sizeDelta = fullscreenTextSize;
        } else
        {
            fullscreenText.text = "Change to Windowed Mode";
            fullscreenText.rectTransform.sizeDelta = windowedModeTextSize;
        }
    }

    public void changeFullscreen()
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
        Screen.fullScreen = !Screen.fullScreen;
    }
}