using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private Canvas menu = null;
    [SerializeField] private string volume = "";

    private Slider slider;
    private bool lowering = false;
    private bool increasing = false;

    void Start()
    {
        slider = GetComponent<Slider>();
        if (PlayerPrefs.HasKey(volume))
        {
            slider.value = PlayerPrefs.GetFloat(volume);
        } else
        {
            slider.value = 1;
        }
    }

    void Update()
    {
        if (increasing)
        {
            slider.value += 0.005f;
        } else if (lowering)
        {
            slider.value -= 0.005f;
        }
        audioMixer.SetFloat(volume, Mathf.Log10(slider.value) * 20);
    }

    public void controllerAdjust(bool i, bool l)
    {
        if (menu)
        {
            if (menu.enabled)
            {
                increasing = i;
                lowering = l;
            }
        } else
        {
            increasing = i;
            lowering = l;
        }
    }

    public void controllerCancel()
    {
        increasing = false;
        lowering = false;
    }

    public void setVolume()
    {
        PlayerPrefs.SetFloat(volume, slider.value);
        PlayerPrefs.Save();
    }
}