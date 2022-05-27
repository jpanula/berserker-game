using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeValueLoader : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    public void LoadVolumeValues()
    {
        masterVolumeSlider.value = GameManager.Instance.MasterVolume;
        sfxVolumeSlider.value = GameManager.Instance.SfxVolume;
        musicVolumeSlider.value = GameManager.Instance.MusicVolume;
    }
}
