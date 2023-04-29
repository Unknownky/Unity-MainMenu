using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]//ʹ��Headerʹ��ͬ�ֶ������ķ�Χ����
    [SerializeField] private TMP_Text volumeTextValue = null;//����ֵ
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPromote = null;

    [Header("Levels To Load")]//ʹ��Header������������Inspector����ʾ
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;//���л��ַ�����������savegamedialog

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);//�����NewGameDialog�а���yes������µĳ���
    }

    public void LoadGameDialogYes()//����б����ļ��Ļ�����ر���ĳ�����û�о͵���NoSavedGameDialog
    {
        if (PlayerPrefs.HasKey("SavedLevel"))//����Ƿ��б������Ϸ
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");//PlayerPrefsΪ���ƫ��������Ϊ���õ�����
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);//��������
        }
    }

    public void ExitButton()//�˳���Ϸ
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;//ͨ���ı�AudioListener(��Ƶ������)��volumeֵ���ñ�������Ϸ��������С
        volumeTextValue.text = volume.ToString("0.0");//��float volume ����һλС����ʾΪ��ǰ����Text��text����
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //��AudioListener.volume��ֵ����Ϊ"masterVolume"�����ļ������ƫ��
        StartCoroutine(ConfirmationBox());//��ʼЯ��
    }

    public void ResetButton(string MenuType)
    { 
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;//����������Ϊ��ʼֵ
            volumeTextValue.text = defaultVolume.ToString("0.0");//�ı�ֵ����
            volumeSlider.value = defaultVolume;//����ֵ����
            VolumeApply();//�ٴε��ý��б����ֹ����
        }
    
    
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPromote.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPromote.SetActive(false);
    }

}
