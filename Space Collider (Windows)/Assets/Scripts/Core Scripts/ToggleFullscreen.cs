using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick = null;

    private AudioSource audioSource;
    private Text fullscreenText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        fullscreenText = GetComponent<Text>();
    }

    void Update()
    {
        if (!Screen.fullScreen)
        {
            fullscreenText.text = "Change to Fullscreen";
            fullscreenText.rectTransform.sizeDelta = new Vector2(224, 24);
        } else
        {
            fullscreenText.text = "Change to Windowed Mode";
            fullscreenText.rectTransform.sizeDelta = new Vector2(297, 24);
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