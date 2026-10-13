using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;
    int max_masterVolume;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", max_masterVolume));
    }

    public void SetVolume(float _value)
    {
        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / max_masterVolume) * 1/5*max_masterVolume);
    }

    public void RefreshSlider(float _value)
    {
        soundSlider.value = _value;
    }
}
