using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public enum PausePlayButtonState
{
    Playing,
    Paused
}

[System.Serializable]
public class SimulationStep
{
    [Header("Content Settings")]
    [TextArea(3, 6)] public string descriptionText;
    public AudioClip descriptionAudio;
    public float timeToNextStep = 5;

    [Header("Camera Settings")]
    public Transform newCameraPosition;
    public Transform newCameraLookAt;
    [Space]
    public float minCameraDistance;
    public float maxCameraDistance;
    [Space]
    public float cameraTransitionTime = 1;
    public float cameraLookAtTime = 1;

    [Header("Events")]
    public UnityEvent onEnterStep;
    public UnityEvent onExitStep;
}

public class SimulationManager : MonoBehaviour
{
    [Header("General and Text Settings")]
    [SerializeField] GameObject stepsPanel;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtDescription;

    [Header("Simulation Settings")]
    [SerializeField] float timeBetweenSteps;
    [SerializeField] bool restartSceneOnFinishSimulation = false;
    bool _simulationRunning;
    public PausePlayButtonState _playPauseButtonState;

    [Header("On Simulation Finish Settings")]
    [SerializeField] Transform cameraPositionOnFinishSimulation;
    [SerializeField] Transform cameraLookAtOnFinishSimulation;
    [SerializeField] float minCameraDistance;
    [SerializeField] float maxCameraDistance;
    /// <summary>
    /// You can use this UnityEvent to active extra methods along the code
    /// </summary>
    [Header("Events Settings")]
    [SerializeField] UnityEvent onBeginSimulation;
    [SerializeField] UnityEvent onFinishSimulation;
    [SerializeField] SimulationStep[] steps;

    [Header("Control Panel")]
    [SerializeField] Button btnPauseStep;
    [SerializeField] Button btnNextStep;
    [SerializeField] Button btnPreviousStep;

    [Header("Progress Bar and Icons")]
    [SerializeField] Image progressBar;
    [SerializeField] Sprite sprPlay;
    [SerializeField] Sprite sprPause;
    IEnumerator buttonsCooldownCoroutine;

    bool isCicling;
    [SerializeField] private int stepIndex = 1;

    private void OnEnable()
    {
        AudioManager.onFinishAudio += AudioFinished;
    }

    private void OnDisable()
    {
        AudioManager.onFinishAudio -= AudioFinished;
    }

    private void Start()
    {
        //StopAllCoroutines();
        btnNextStep.onClick.AddListener(NextStep);
        btnPreviousStep.onClick.AddListener(PreviousStep);
        btnPauseStep.onClick.AddListener(PlayPause);
        AddCameraControllerComponent();
        buttonsCooldownCoroutine = CooldownOnButtonOnNextStep();
    }

    void AddCameraControllerComponent()
    {
        CameraController _controller = Camera.main.GetComponent<CameraController>();
        if (!_controller)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
    }

    void OpenStepsPanel()
    {
        stepsPanel.SetActive(true);
    }

    void CloseStepsPanel()
    {
        stepsPanel.SetActive(false);
    }

    public void StartSimulation(SimulationStep[] _steps)
    {
        steps = _steps;
        onBeginSimulation?.Invoke();
        SetStep(steps[stepIndex]);
        UIManager.instance.HideAllButtons();
        progressBar.fillAmount = 0;

        OpenStepsPanel();
        _simulationRunning = true;
        AudioManager.instance.PlayRandomSong(MusicStyle.SimulationMusic);

        // Force simulation button to change it's sprite
        UIManager.instance.btnSimulation.GetComponent<Image>().sprite =
            UIManager.instance.btnSimulation.GetComponent<SubPanelButton>().selectedSprite;
    }

    void SetStep(SimulationStep _nextStep)
    {
        StopAllCoroutines();

        timeBetweenSteps = _nextStep.timeToNextStep;

        if (_nextStep.descriptionText == "")
        {
            NextStep();
            return;
        }

        txtTitle.DOFade(0, 1).OnComplete(() =>
        {
            txtTitle.text = $"Passo {stepIndex}";
            txtTitle.DOFade(1, 1);
        });

        txtDescription.DOFade(0, 1).OnComplete(() =>
        {
            txtDescription.text = _nextStep.descriptionText;
            txtDescription.DOFade(1, 1);
        });

        float delayToNextStep = timeBetweenSteps;

        if (_nextStep.descriptionAudio != null)
        {
            AudioManager.instance.StopAudio();
            AudioManager.instance.PlayAudio(_nextStep.descriptionAudio);
            delayToNextStep = AudioManager.instance.GetCurrentAudioLength() + (delayToNextStep / 2);
        }

        SetupCamera(_nextStep);
        steps[stepIndex].onEnterStep?.Invoke();

        //if (!isCicling)
        //    StartCoroutine(CicleBetweenSteps(delayToNextStep));

        progressBar.DOFillAmount((float)(stepIndex + 1) / (float)steps.Length, 1);
        ChangePlayPauseButtonState(PausePlayButtonState.Playing);
        Time.timeScale = 1;

        btnNextStep.interactable = false;
        btnPreviousStep.interactable = false;
        btnPauseStep.interactable = false;

        Invoke("EnablePanelButtons", 1);
    }

    void EnablePanelButtons()
    {
        btnNextStep.interactable = true;
        btnPreviousStep.interactable = true;
        btnPauseStep.interactable = true;
    }

    void PlayPause()
    {
        // If the audio is still playing, toggle it and change _playPauseButtonState state
        if (!AudioManager.instance.IsAudioFinished())
        {
            AudioManager.instance.ToggleAudio();

            if (AudioManager.instance.IsAudioPlaying())
            {
                ChangePlayPauseButtonState(PausePlayButtonState.Playing);
                Time.timeScale = 1;
            }
            else
            {
                ChangePlayPauseButtonState(PausePlayButtonState.Paused);
                Time.timeScale = 0;
            }
        }
        else // if not, decide if should kill countdown coroutine or go to the next step
        {
            if (_playPauseButtonState == PausePlayButtonState.Paused)
            {
                //NextStep();
                ChangePlayPauseButtonState(PausePlayButtonState.Playing);
                Time.timeScale = 1;
            }
            else
            {
                //StopAllCoroutines();
                ChangePlayPauseButtonState(PausePlayButtonState.Paused);
                Time.timeScale = 0;
            }
        }
    }

    void PreviousStep()
    {
        stepIndex--;

        if (stepIndex <= 0)
        {
            FinishSimulation();
        }
        else
        {
            SetStep(steps[stepIndex]);
        }
    }

    void NextStep()
    {
        // steps[stepIndex].onExitStep?.Invoke();

        stepIndex++;
        print("Passou3");
        if (stepIndex < steps.Length)
        {
            print("Passou4");
            SetStep(steps[stepIndex]);
            print("Passou5");
        }
        else
        {
            FinishSimulation();
        }
        print("Passou6");
    }

    void FinishSimulation()
    {
        UIManager.instance.btnSimulation.GetComponent<Image>().sprite =
            UIManager.instance.btnSimulation.GetComponent<SubPanelButton>().unselectedSprite;

        onFinishSimulation?.Invoke();

        stepIndex = 1;
        //ResetCamera();
        isCicling = false;
        //UIManager.instance.EnableAllButtons();
        //UIManager.instance.OpenMainPanel();
        _simulationRunning = false;
        //AudioManager.instance.StopAudio();
        //AudioManager.instance.PlayRandomSong(MusicStyle.GeneralMusic);

        if (restartSceneOnFinishSimulation)
        {
            StopAllCoroutines();
            DOTween.KillAll();

            string sceneName = SceneManager.GetActiveScene().name;
            UIManager.instance.FadePanel(1, 1, sceneName);
        }
        else
        {
            CloseStepsPanel();
        }
    }

    void AudioFinished()
    {
        print("Passou1");
        StartCoroutine(CountdownToNextStep());
        print("Passou2");
    }

    IEnumerator CountdownToNextStep()
    {
        yield return new WaitForSeconds(timeBetweenSteps);
        NextStep();
    }

    IEnumerator CooldownOnButtonOnNextStep()
    {
        btnNextStep.interactable = false;
        btnPreviousStep.interactable = false;
        btnPauseStep.interactable = false;

        yield return new WaitForSeconds(1);

        btnNextStep.interactable = true;
        btnPreviousStep.interactable = true;
        btnPauseStep.interactable = true;
    }

    //IEnumerator CicleBetweenSteps(float _delayToNextStep)
    //{
    //    isCicling = true;
    //    while (stepIndex < steps.Length)
    //    {
    //        yield return new WaitForSeconds(_delayToNextStep);

    //        if (stepIndex < steps.Length - 1)
    //            NextStep();
    //        else
    //            break;
    //    }

    //    FinishSimulation();
    //}

    void ResetCamera()
    {
        GameObject cameraGO = Camera.main.gameObject;
        //FreeCamera freeCamera = Camera.main.gameObject.GetComponent<FreeCamera>();
        CameraController _cameraController = cameraGO.GetComponent<CameraController>();

        // freeCamera.enabled = false;
        _cameraController.MoveCameraTo(cameraPositionOnFinishSimulation, cameraLookAtOnFinishSimulation, minCameraDistance, maxCameraDistance);
        //freeCamera.enabled = false;

        //if (cameraPositionOnFinishSimulation != null)
        //    cameraGO.transform.position = cameraPositionOnFinishSimulation.position;

        //// Avoid camera stagerring
        //if (freeCamera.target != cameraLookAtOnFinishSimulation)
        //{
        //    if (cameraPositionOnFinishSimulation != null)
        //        freeCamera.target = cameraLookAtOnFinishSimulation;
        //}

        //freeCamera.MinDistance = minCameraDistance;
        //freeCamera.MaxDistance = maxCameraDistance;

        //freeCamera.RenableFreeCameraSystem();
    }

    void SetupCamera(SimulationStep _step)
    {
        GameObject cameraGO = Camera.main.gameObject;
        CameraController _cameraController = cameraGO.GetComponent<CameraController>();

        // freeCamera.enabled = false;
        _cameraController.MoveCameraTo(_step.newCameraPosition, _step.newCameraLookAt, _step.minCameraDistance, _step.maxCameraDistance);
        // freeCamera.target = _step.newCameraLookAt;

        //if (_step.newCameraPosition != null)
        //{
        //    Sequence MySeq = DOTween.Sequence();
        //    MySeq.Append(cameraGO.transform.DOMove(_step.newCameraPosition.position, _step.cameraTransitionTime));
        //    if (_step.newCameraLookAt != null)
        //    {
        //        MySeq.Append(cameraGO.transform.DOLookAt(freeCamera.GetDestination(), _step.cameraLookAtTime));
        //    }

        //    MySeq.OnComplete(() => freeCamera.ReactiveFreeCameraSystem());
        //}

        // freeCamera.MinDistance = _step.minCameraDistance;
        //freeCamera.MaxDistance = _step.maxCameraDistance;

        //StartCoroutine(SetFreeCameraDistance(_step.minCameraDistance, _step.maxCameraDistance, _step.newCameraLookAt, freeCamera));
    }

    //IEnumerator SetFreeCameraDistance(float _min, float _max, Transform _lookAt, FreeCamera _freeCamera)
    //{
    //    yield return new WaitForSeconds(1);
    //    _freeCamera.transform.DOLookAt(_lookAt.position, 1).OnComplete(() =>
    //    {
    //        _freeCamera.target = _lookAt;
    //        _freeCamera.MinDistance = _min;
    //        _freeCamera.MaxDistance = _max;
    //    });
    //}

    public bool IsSumilationRunning()
    {
        return _simulationRunning;
    }

    void ChangePlayPauseButtonState(PausePlayButtonState _state)
    {
        switch (_state)
        {
            case PausePlayButtonState.Playing:
                {
                    _playPauseButtonState = PausePlayButtonState.Playing;
                    btnPauseStep.GetComponent<Image>().sprite = sprPause;
                    break;
                }
            case PausePlayButtonState.Paused:
                {
                    _playPauseButtonState = PausePlayButtonState.Paused;
                    btnPauseStep.GetComponent<Image>().sprite = sprPlay;
                    break;
                }
        }
    }
}
