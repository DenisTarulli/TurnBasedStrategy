using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        closeButton.onClick.AddListener(Hide);
    }

    private void OnEnable()
    {
        // Al abrir el panel, sincronizamos los sliders con el volumen actual guardado
        musicSlider.SetValueWithoutNotify(SoundManager.Instance.GetMusicVolume());
        sfxSlider.SetValueWithoutNotify(SoundManager.Instance.GetSfxVolume());
    }

    private void OnMusicSliderChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
