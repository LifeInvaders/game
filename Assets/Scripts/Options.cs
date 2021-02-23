using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    void Start()
    {
        float volume;
        soundSystem.GetFloat("volume",out volume);
        volumeSlider.value = volume;
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        fullscreenToggle.isOn = Screen.fullScreen;
        soundSystem.FindMatchingGroups("Master");
    }
    [SerializeField] private AudioMixer soundSystem;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;



    public void Fullscreen(bool check) => Screen.fullScreen = check;

    public void Quality(int index) => QualitySettings.SetQualityLevel(index);

    public void SetVolume(float vol) => soundSystem.SetFloat("volume", vol);
}
