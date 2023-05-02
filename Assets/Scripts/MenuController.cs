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

    [Header("Gameplay Settings")]//����Gameplay
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;
    [SerializeField] private int defaultSenSlider = 4;
    public int mainControllerSen = 4;//���������ű��ķ���,Ҳ��������Volume����

    [Header("Toggle Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text BrightnessTextValue = null;
    [SerializeField] private Slider BrightnessSlider = null;
    [SerializeField] private float defaultBrightness = 1.0f;//���ȱ仯��ʾ���ı���Ҫ���л�������������Slider�ؼ���Ĭ������(����reset)

    [Space(10)]
    [SerializeField] private TMP_Dropdown qulityDropdown = null;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private float _brightnessLevel;
    private bool _isFullScreen;//���ڽ���Ӧ�ú���Ҫ�������ű�����ʹ�ã����ʹ��private����


    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPromote = null;

    [Header("Levels To Load")]//ʹ��Header������������Inspector����ʾ
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;//���л��ַ�����������savegamedialog


    [Header("Resolution Setting")]
    public TMP_Dropdown resolutionDropdown = null;//������Ҫ�ڽű��жԸ�������е��������Ҫ�����Dropdown����Ӧ��Qualityֻ��Ҫ��������
    private Resolution[] resolutions;

    private void Start()//��ʼ������
    {
        resolutions = Screen.resolutions;//��ȡ���ݵ�ǰ��Ļ��ȡ��ȫ���ֱ�������
        resolutionDropdown.ClearOptions();//���ѡ���������»�ȡ����string�б����ӣ����ǹ̶��Ĳ���

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }//���ϻ�ȡoptions�б�������Dropdown��ʾ;��ȡ��CurrenResolutionIndex���ڵ�ǰ��������ʾ;��Ҫʱ����List��

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)//���÷������ص�Dropdown���Ե�����Ļ�ķֱ���
    {
        Resolution resolution = resolutions[resolutionIndex];//��ȡDropdown��ʹ�õ�Index��Ӧ�ķֱ��ʽṹ��
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);//���ݵ�ǰ��Ļ�Ƿ�Ϊȫ�������÷ֱ���
    }
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

    public void SetVolume(float volume)//����Slider�Ķ�̬����
    {//����������Ҫ�ڻ�������ʱ�۲��С�����ÿһ�α仯�ж���ҪӦ�õ�AudioListener��
        AudioListener.volume = volume;//ͨ���ı�AudioListener(��Ƶ������)��volumeֵ���ñ�������Ϸ��������С
        volumeTextValue.text = volume.ToString("0.0");//��float volume ����һλС����ʾΪ��ǰ����Text��text����
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);//���������һ����Ҫ����Ϸ�н������飬�������mainControllerSen��������������Ϸ��Ӧ��
        ControllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void SetBrighness(float brightness)
    {
        _brightnessLevel = brightness;
        BrightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;//��������Ϊ��Graphics Apply��Ӧ�õ���Ϸ��
    }

    public void SetQuality(int qualityIndex)//��ȡ��������������ӦBuilding�еĻ�������
    {
        _qualityLevel = qualityIndex;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //��AudioListener.volume��ֵ����Ϊ"masterVolume"�����ļ������ƫ��
        StartCoroutine(ConfirmationBox());//��ʼЯ��
    }


    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);//���Y�ᷴתToggle����ô���ƫ���б�����һ��Ϊ��
            //InvertY ison ʵ�ʶ���Ϸ�ĸı����д�ں���
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);//masterInvertY���浽ƫ��������Ϊ��
            //not InvertY ͬ����������������
        }

        PlayerPrefs.SetFloat("masterSens", mainControllerSen);//��mainControllerSen��ֵ��float������ƫ���С�
        StartCoroutine(ConfirmationBox());//��ʼЯ��

    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //������������Ҫ���иı����ȵĴ���

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1:0));//û��SetBool,using SetInt in place
        Screen.fullScreen = _isFullScreen;

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);//����Building�����õ�ѡ�������SceneManager

        StartCoroutine(ConfirmationBox());//��ʼЯ��
    }
    public void ResetButton(string MenuType)//ʹ�ø÷���ʱ����Ҫ����һ��string
    { 
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;//����������Ϊ��ʼֵ
            volumeTextValue.text = defaultVolume.ToString("0.0");//�ı�ֵ����
            volumeSlider.value = defaultVolume;//����ֵ����
            VolumeApply();//�ٴε��ý��б����ֹ����
        }
        else if(MenuType == "Gameplay")
        {
            //PlayerPrefs.SetFloat("masterSens", defaultSenSlider);//Sens����ΪĬ��ֵ
            //PlayerPrefs.SetInt("masterInvertY", 0);//InvertY����Ϊ0��false��   ��������GameApply�е�����**
            ControllerSenTextValue.text = defaultSenSlider.ToString("0");//Sens�ı�ֵ����ΪdefaultSenSlider
            ControllerSenSlider.value = defaultSenSlider;//����ֵ����
            mainControllerSen = defaultSenSlider;//����ֵ����
            invertYToggle.isOn = false;//�ر�Y�ᷴת�İ�ť
            GameplayApply();//�����ֹ����
        }
        else if(MenuType == "Graphics")
        {
            _brightnessLevel = defaultBrightness;
            BrightnessTextValue.text = defaultBrightness.ToString("0.0");
            BrightnessSlider.value = defaultBrightness;

            _qualityLevel = 1;
            qulityDropdown.value = _qualityLevel;//1��Ӧδmid������
            QualitySettings.SetQualityLevel(_qualityLevel);

            _isFullScreen = false;
            fullScreenToggle.isOn = false;
            Screen.fullScreen = _isFullScreen;//����Ϊ��Ĭ��ֵ���������ʹ��

            Resolution currentResolution = Screen.currentResolution;//��ȡ��ǰ��Ļ�ķֱ���ΪĬ��ֵ
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;//����DropdownֵΪ��󣬼���ǰ��Ļ�ķֱ���ΪĬ��ֵ
            GraphicsApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPromote.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPromote.SetActive(false);
    }

}
