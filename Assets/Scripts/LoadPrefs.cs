using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//进行默认prefab文件的设置,在这里只需要声明真正需要进行prefab设置的物体，可以通过MenuController中使用PlayerFab的数量进行声明
public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private TMP_Text BrightnessTextValue = null;
    [SerializeField] private Slider BrightnessSlider = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qulityDropdown = null;

    [Header("FullScreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;

    [Header("InvertY Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()//使用Awake使得偏好设置最快进行加载
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))//如果该pref存在
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");//如果没有重置保证不出错
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qulityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullScreen"))
            {
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");

                if (localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                BrightnessTextValue.text = localBrightness.ToString("0.0");
                BrightnessSlider.value = localBrightness;
                //对应改变亮度的代码
            }

            if (PlayerPrefs.HasKey("masterSens"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("masterSens");

                ControllerSenTextValue.text = localSensitivity.ToString("0.0");
                ControllerSenSlider.value = localSensitivity;
                menuController.mainControllerSen = Mathf.RoundToInt(localSensitivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                if (PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }
        }
    }
}
