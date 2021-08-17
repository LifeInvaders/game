using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MainMenu
{
    public class Options : MonoBehaviour
    {
        void Start()
        {
            // Debug.Log($"{PlayerDatabase.Instance.soundLevel},{Mathf.Pow(10,PlayerDatabase.Instance.soundLevel/10)}");
            // soundSystem.SetFloat("volume", Mathf.Pow(10,PlayerDatabase.Instance.soundLevel/10));
            // volumeSlider.value = PlayerDatabase.Instance.soundLevel;
            if (loadSettings)
            {
                QualitySettings.SetQualityLevel(PlayerDatabase.Instance.qualitySetting);
                qualityDropdown.value = QualitySettings.GetQualityLevel();
                fullscreenToggle.isOn = Screen.fullScreen;
                soundSystem.FindMatchingGroups("Master");
            }
            SetDefinitionValues();
            SetFPSValues();
        }

        private void SetFPSValues()
        {
            for (int i = 0; i < _fpsValues.Length; i++)
                if (_fpsValues[i] == Application.targetFrameRate)
                    fpsDropdown.value = i;
        }

        private void SetDefinitionValues()
        {
            List<string> options = new List<string>();
            int currentRes = 0;
            for (int i = 0; i < Screen.resolutions.Length; i += 2)
            {
                if (Screen.resolutions[i].width == Screen.width
                    && Screen.resolutions[i].height == Screen.height)
                    currentRes = i / 2;
                string option = Screen.resolutions[i].width.ToString() + "x" + Screen.resolutions[i].height.ToString();
                options.Add(option);
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentRes;
        }

        [SerializeField] private bool loadSettings = false;
        [SerializeField] private AudioMixer soundSystem;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fpsDropdown;

        private static int[] _fpsValues = {30, 60, 120, -1};

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

        public void SetFps(int index) => Application.targetFrameRate = _fpsValues[index];

        public void SetResolution(int index) =>
            Screen.SetResolution(Screen.resolutions[index * 2].width, Screen.resolutions[index * 2].height,
                Screen.fullScreen);
    }
}