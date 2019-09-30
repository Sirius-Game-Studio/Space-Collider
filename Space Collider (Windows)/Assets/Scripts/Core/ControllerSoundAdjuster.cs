using UnityEngine;

public class ControllerSoundAdjuster : MonoBehaviour
{
    [SerializeField] private SetVolume volumeSlider = null;

    private Controls input;

    private void Awake()
    {
        input = new Controls();
    }

    void OnEnable()
    {
        input.Enable();
        input.Sound.LowerSound.performed += context => volumeSlider.controllerAdjust(false, true);
        input.Sound.IncreaseSound.performed += context => volumeSlider.controllerAdjust(true, false);
        input.Sound.LowerSound.canceled += context => volumeSlider.controllerCancel();
        input.Sound.IncreaseSound.canceled += context => volumeSlider.controllerCancel();
    }

    void OnDisable()
    {
        input.Disable();
        input.Sound.LowerSound.performed -= context => volumeSlider.controllerAdjust(false, true);
        input.Sound.IncreaseSound.performed -= context => volumeSlider.controllerAdjust(true, false);
        input.Sound.LowerSound.canceled -= context => volumeSlider.controllerCancel();
        input.Sound.IncreaseSound.canceled -= context => volumeSlider.controllerCancel();
    }
}
