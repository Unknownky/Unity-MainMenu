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

    [Header("Gameplay Settings")]//设置Gameplay
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;
    [SerializeField] private int defaultSenSlider = 4;
    public int mainControllerSen = 4;//用于其他脚本的访问,也可以用于Volume设置

    [Header("Toggle Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text BrightnessTextValue = null;
    [SerializeField] private Slider BrightnessSlider = null;
    [SerializeField] private float defaultBrightness = 1.0f;//亮度变化显示的文本需要序列化的声明，亮度Slider控件，默认亮度(用于reset)

    [Space(10)]
    [SerializeField] private TMP_Dropdown qulityDropdown = null;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private float _brightnessLevel;
    private bool _isFullScreen;//由于进行应用后不需要在其他脚本进行使用，因此使用private修饰


    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPromote = null;

    [Header("Levels To Load")]//使用Header可以让文字在Inspector中显示
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;//序列化字符，用来控制savegamedialog


    [Header("Resolution Setting")]
    public TMP_Dropdown resolutionDropdown = null;//由于需要在脚本中对该物体进行调整因此需要传入该Dropdown；对应的Quality只需要单个方法
    private Resolution[] resolutions;

    private void Start()//开始即进行
    {
        resolutions = Screen.resolutions;//获取根据当前屏幕获取的全部分辨率数组
        resolutionDropdown.ClearOptions();//清空选项用于以下获取到的string列表的添加，这是固定的操作

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
        }//以上获取options列表以用于Dropdown显示;获取的CurrenResolutionIndex用于当前索引的显示;主要时基于List的

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)//将该方法挂载到Dropdown中以调整屏幕的分辨率
    {
        Resolution resolution = resolutions[resolutionIndex];//获取Dropdown中使用的Index对应的分辨率结构体
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);//根据当前屏幕是否为全屏来设置分辨率
    }
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

    public void SetVolume(float volume)//用于Slider的动态调用
    {//由于音量需要在滑动是随时观察大小因此在每一次变化中都需要应用到AudioListener中
        AudioListener.volume = volume;//通过改变AudioListener(音频监听器)的volume值来该变整个游戏的音量大小
        volumeTextValue.text = volume.ToString("0.0");//将float volume 保留一位小数显示为当前挂载Text的text内容
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);//鼠标灵敏度一般需要在游戏中进行体验，因此先用mainControllerSen保存下来再在游戏中应用
        ControllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void SetBrighness(float brightness)
    {
        _brightnessLevel = brightness;
        BrightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;//这里设置为在Graphics Apply中应用到游戏中
    }

    public void SetQuality(int qualityIndex)//获取下拉条的索引对应Building中的画质设置
    {
        _qualityLevel = qualityIndex;
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //将AudioListener.volume该值保存为"masterVolume"该名文件的玩家偏好
        StartCoroutine(ConfirmationBox());//开始携程
    }


    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);//如果Y轴反转Toggle打开那么玩家偏好中保存这一项为真
            //InvertY ison 实际对游戏的改变可以写在后面
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);//masterInvertY保存到偏好中设置为假
            //not InvertY 同样可以在这后面添加
        }

        PlayerPrefs.SetFloat("masterSens", mainControllerSen);//将mainControllerSen的值以float保存在偏好中。
        StartCoroutine(ConfirmationBox());//开始携程

    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //下面可以添加需要进行改变亮度的代码

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1:0));//没有SetBool,using SetInt in place
        Screen.fullScreen = _isFullScreen;

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);//调用Building中设置的选项，类似与SceneManager

        StartCoroutine(ConfirmationBox());//开始携程
    }
    public void ResetButton(string MenuType)//使用该方法时就需要给出一个string
    { 
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;//将音量设置为初始值
            volumeTextValue.text = defaultVolume.ToString("0.0");//文本值重置
            volumeSlider.value = defaultVolume;//滑块值重置
            VolumeApply();//再次调用进行保存防止出错
        }
        else if(MenuType == "Gameplay")
        {
            //PlayerPrefs.SetFloat("masterSens", defaultSenSlider);//Sens设置为默认值
            //PlayerPrefs.SetInt("masterInvertY", 0);//InvertY设置为0（false）   这两行在GameApply中调用了**
            ControllerSenTextValue.text = defaultSenSlider.ToString("0");//Sens文本值设置为defaultSenSlider
            ControllerSenSlider.value = defaultSenSlider;//滑块值重置
            mainControllerSen = defaultSenSlider;//声明值重置
            invertYToggle.isOn = false;//关闭Y轴反转的按钮
            GameplayApply();//保存防止出错
        }
        else if(MenuType == "Graphics")
        {
            _brightnessLevel = defaultBrightness;
            BrightnessTextValue.text = defaultBrightness.ToString("0.0");
            BrightnessSlider.value = defaultBrightness;

            _qualityLevel = 1;
            qulityDropdown.value = _qualityLevel;//1对应未mid的索引
            QualitySettings.SetQualityLevel(_qualityLevel);

            _isFullScreen = false;
            fullScreenToggle.isOn = false;
            Screen.fullScreen = _isFullScreen;//设置为了默认值，后面可以使用

            Resolution currentResolution = Screen.currentResolution;//获取当前屏幕的分辨率为默认值
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;//调整Dropdown值为最大，即当前屏幕的分辨率为默认值
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
