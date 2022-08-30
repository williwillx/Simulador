using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static event Action<GameObject> OnSubPanelOpened;
    public static event Action<GameObject> OnSubMenuPanelOpened;
    public static event Action OnStartButtonPushed;

    [Header("Panels")]
    [SerializeField] GameObject startPanel;

    [Header("Intro Panel")]
    [SerializeField] Button btnStart;

    [Header("Main panel")]
    [SerializeField] SubPanelButton btnEngine;
    public SubPanelButton btnSimulation;
    [SerializeField] SubPanelButton btnCar;
    [SerializeField] SubPanelButton btnView;

    [Header("Config Panel")]
    [SerializeField] Toggle audioToggle;
    [SerializeField] Toggle musicToggle;
    [Space]
    [SerializeField] Slider audioSlider;
    [SerializeField] Slider musicSlider;

    [Header("Help Panel")]
    [SerializeField] Text txtHelpPanelTitle;
    [SerializeField] Text txtHelpPanelDescription;
    [SerializeField] ScrollRect helpPanelScrollRect;
    [SerializeField] GameObject[] allHelpPanels;

    [Header("Others")]
    [SerializeField] Image fadeImage;
    private float timeFade = 1;

    private void Start()
    {
        InitStartPanel();
    }

    void InitStartPanel()
    {
        Image startPanelBackground = btnStart.transform.parent.GetComponent<Image>();
        Image btnStartImage = btnStart.GetComponent<Image>();

        Sequence seq = DOTween.Sequence();

        seq.Insert(0, startPanelBackground.DOFade(0, 0));
        seq.Insert(0, btnStartImage.DOFade(0, 0));
        seq.Insert(1, startPanelBackground.DOFade(1, 4));
        seq.Insert(2, btnStartImage.DOFade(1, 3));

        seq.OnComplete(() => btnStart.interactable = true);


        if (VerifyFirstScene.instance.isOpened)
        {
            //timeFade = 0.f;
            Invoke("ToggleIntroPanel", 0.2f);
        }
        else
        {
            //timeFade = 1f;
            btnStart.onClick.AddListener(ToggleIntroPanel);
        }

    }

    public void ToggleIntroPanel()
    {

        if (startPanel.activeInHierarchy)
        {
            OnStartButtonPushed?.Invoke();

        }

        startPanel.GetComponent<CanvasGroup>().DOFade(0, timeFade).OnComplete(() =>
        {
            startPanel.SetActive(!startPanel.activeInHierarchy);
            FadePanel(0, 1);
        });

        SetInitialConfigValues();
        OpenMainPanel();
        AudioManager.instance.FadeOutBackgroundTrack();
        VerifyFirstScene.instance.isOpened = true;
    }

    public void OpenMainPanel()
    {
        btnSimulation.ToggleSubPanelManual(true);
        FindObjectOfType<CameraController>().InitCamera();
    }

    public void OnPanelOpened(GameObject _panelOpened)
    {
        OnSubPanelOpened?.Invoke(_panelOpened);
    }

    public void OnSubMenuOpened(GameObject _panelOpened)
    {
        OnSubMenuPanelOpened?.Invoke(_panelOpened);
    }

    public void CloseAllPanels()
    {
        OnSubPanelOpened?.Invoke(null);
    }

    public void EnableAllButtons()
    {
        btnEngine.EnableButtons();
        btnSimulation.EnableButtons();
        btnCar.EnableButtons();
        btnView.EnableButtons();
    }

    public void HideAllButtons()
    {
        CloseAllPanels();

        btnEngine.DisableButtons();
        btnSimulation.DisableButtons();
        btnCar.DisableButtons();
        btnView.DisableButtons();
    }

    void SetInitialConfigValues()
    {
        // Set initial values
        audioToggle.isOn = PlayerPrefs.GetInt(CONST_PARAMS.PLAYERPREFS_AUDIO_ON_OFF) == 1 ? true : false;
        musicToggle.isOn = PlayerPrefs.GetInt(CONST_PARAMS.PLAYERPREFS_MUSIC_ON_OFF) == 1 ? true : false;
        audioSlider.value = PlayerPrefs.GetFloat(CONST_PARAMS.PLAYERPREFS_AUDIO_VOLUME);
        musicSlider.value = PlayerPrefs.GetFloat(CONST_PARAMS.PLAYERPREFS_MUSIC_VOLUME);

        // Add Listeners
        audioSlider.onValueChanged.AddListener((x)
           =>
        {
            PlayerPrefsManager.instance.SetFloatPlayerPrefs(CONST_PARAMS.PLAYERPREFS_AUDIO_VOLUME, x);
            AudioManager.instance.SetAudioVolume(x);
        });
        musicSlider.onValueChanged.AddListener((x)
            =>
        {
            PlayerPrefsManager.instance.SetFloatPlayerPrefs(CONST_PARAMS.PLAYERPREFS_MUSIC_VOLUME, x);
            AudioManager.instance.SetMusicVolume(x);
        });

        audioToggle.onValueChanged.AddListener((x) => AudioManager.instance.ToggleAudio(x));
        musicToggle.onValueChanged.AddListener((x) => AudioManager.instance.ToggleMusic(x));
    }

    public void ChangeHelpPanelTexts(string _title, GameObject _panelToOpen)
    {
        DisableAllHelpPanels();
        txtHelpPanelTitle.text = _title;
        _panelToOpen.SetActive(true);
        helpPanelScrollRect.content = _panelToOpen.GetComponent<RectTransform>();
    }

    void DisableAllHelpPanels()
    {
        foreach (var painel in allHelpPanels)
        {
            painel.SetActive(false);
        }
    }

    public void FadePanel(float _finalValue, float _fadeDuration, string _sceneToLoad = "")
    {
        fadeImage.DOFade(_finalValue, _fadeDuration).OnComplete(() =>
        {
            if (_sceneToLoad != "")
            {
                StartCoroutine(LoadScene(_sceneToLoad));
            }
        });
    }

    IEnumerator LoadScene(string scene)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {

                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ChangeScene(string scene)
    {
        FadePanel(1, 0.5f, scene);
        if (!VerifyFirstScene.instance.isOpened)
        {
            VerifyFirstScene.instance.isOpened = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}