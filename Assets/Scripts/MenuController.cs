using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]//使用Header使不同字段所属的范围清晰
    [SerializeField] private TMP_Text volumeTextValue = null;//赋初值
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPromote = null;

    [Header("Levels To Load")]//使用Header可以让文字在Inspector中显示
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;//序列化字符，用来控制savegamedialog

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);//如果在NewGameDialog中按了yes则加载新的场景
    }

    public void LoadGameDialogYes()//如果有保存文件的话则加载保存的场景，没有就弹出NoSavedGameDialog
    {
        if (PlayerPrefs.HasKey("SavedLevel"))//检测是否有保存的游戏
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");//PlayerPrefs为玩家偏好器，即为设置的数据
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);//弹出窗口
        }
    }

    public void ExitButton()//退出游戏
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;//通过改变AudioListener(音频监听器)的volume值来该变整个游戏的音量大小
        volumeTextValue.text = volume.ToString("0.0");//将float volume 保留一位小数显示为当前挂载Text的text内容
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //将AudioListener.volume该值保存为"masterVolume"该名文件的玩家偏好
        StartCoroutine(ConfirmationBox());//开始携程
    }

    public void ResetButton(string MenuType)
    { 
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;//将音量设置为初始值
            volumeTextValue.text = defaultVolume.ToString("0.0");//文本值重置
            volumeSlider.value = defaultVolume;//滑块值重置
            VolumeApply();//再次调用进行保存防止出错
        }
    
    
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPromote.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPromote.SetActive(false);
    }

}
