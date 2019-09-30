using UnityEngine;

public class ControllerMusicAdjuster : MonoBehaviour
{
    [SerializeField] private SetVolume volumeSlider = null;

    private Controls input;

    void Awake()
    {
        input = new Controls();
    }

    void OnEnable()
    {
        input.Enable();
        input.Sound.LowerMusic.performed += context => volumeSlider.controllerAdjust(false, true);
        input.Sound.IncreaseMusic.performed += context => volumeSlider.controllerAdjust(true, false);
        input.Sound.LowerMusic.canceled += context => volumeSlider.controllerCancel();
        input.Sound.IncreaseMusic.canceled += context => volumeSlider.controllerCancel();
    }

    void OnDisable()
    {
        input.Disable();
        input.Sound.LowerMusic.performed -= context => volumeSlider.controllerAdjust(false, true);
        input.Sound.IncreaseMusic.performed -= context => volumeSlider.controllerAdjust(true, false);
        input.Sound.LowerMusic.canceled -= context => volumeSlider.controllerCancel();
        input.Sound.IncreaseMusic.canceled -= context => volumeSlider.controllerCancel();
    }
}
