using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    void Start()
    {
        // Debug.Log($"{PlayerDatabase.Instance.soundLevel},{Mathf.Pow(10,PlayerDatabase.Instance.soundLevel/10)}");
        // soundSystem.SetFloat("volume", Mathf.Pow(10,PlayerDatabase.Instance.soundLevel/10));
        // volumeSlider.value = PlayerDatabase.Instance.soundLevel;
        QualitySettings.SetQualityLevel(PlayerDatabase.Instance.qualitySetting);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        fullscreenToggle.isOn = Screen.fullScreen;
        soundSystem.FindMatchingGroups("Master");
        List<string> options = new List<string>();
        int currentRes = 0;
        for(int i = 0; i < Screen.resolutions.Length; i += 2)
        {
            if (Screen.resolutions[i].width == Screen.width
                && Screen.resolutions[i].height == Screen.height)
                currentRes = i/2;
            string option =Screen.resolutions[i].width.ToString() + "x" + Screen.resolutions[i].height.ToString();
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentRes;
    }
    [SerializeField] private AudioMixer soundSystem;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;



    public void Fullscreen(bool check) => Screen.fullScreen = check;

    public void Quality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerDatabase.Instance.qualitySetting = index;
    }

    public void SetVolume(float vol)
    {
        PlayerDatabase.Instance.soundLevel = vol;
        soundSystem.SetFloat("volume", vol);
        // soundSystem.SetFloat("volume", Mathf.Pow(10,PlayerDatabase.Instance.soundLevel/10));
    }

    public void SetResolution(int index) =>
        Screen.SetResolution(Screen.resolutions[index * 2].width, Screen.resolutions[index * 2].height, Screen.fullScreen);
}
