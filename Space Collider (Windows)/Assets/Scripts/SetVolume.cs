using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private string volume = "";
    [Tooltip("Xbox/PS Controller only.")] [SerializeField] private bool isBumper = false;

    private Slider slider;
    private KeyCode[] joystickButtons;

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
        joystickButtons = new KeyCode[2];
        if (isBumper) //Xbox is LB & RB, PS is L1 & R1
        {
            joystickButtons[0] = KeyCode.JoystickButton4;
            joystickButtons[1] = KeyCode.JoystickButton5;
        } else //Xbox is LT & RT, PS is L2 & R2
        {
            joystickButtons[0] = KeyCode.JoystickButton6;
            joystickButtons[1] = KeyCode.JoystickButton7;
        }
    }

    void Update()
    {
        audioMixer.SetFloat(volume, Mathf.Log10(slider.value) * 20);
        if (joystickButtons.Length == 2)
        {
            if (Input.GetKey(joystickButtons[0]))
            {
                slider.value -= (float)0.001;
            } else if (Input.GetKey(joystickButtons[1]))
            {
                slider.value += (float)0.001;
            }
        }
    }

    public void setVolume()
    {
        PlayerPrefs.SetFloat(volume, slider.value);
        PlayerPrefs.Save();
    }
}
