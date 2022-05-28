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
        masterVolumeSlider.value = GameManager.MasterVolume;
        sfxVolumeSlider.value = GameManager.SfxVolume;
        musicVolumeSlider.value = GameManager.MusicVolume;
    }
}
