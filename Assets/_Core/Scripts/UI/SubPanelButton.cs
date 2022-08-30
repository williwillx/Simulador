using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SubPanelButton : MonoBehaviour
{
    [SerializeField] GameObject myMenuOptions;

    public Sprite unselectedSprite;
    public Sprite selectedSprite;

    [SerializeField] private bool simulationBtn;
    Toggle myToggle;

    private void OnEnable()
    {
        UIManager.OnSubPanelOpened += ToggleSubPanelByEvent;
    }

    private void OnDisable()
    {
        UIManager.OnSubPanelOpened -= ToggleSubPanelByEvent;
    }

    private void Start()
    {
        myToggle = GetComponent<Toggle>();
        myToggle.onValueChanged.AddListener(delegate { UIManager.instance.OnPanelOpened(gameObject); });

        // Guarantee the panel is disabled on start
        ToggleSubPanelManual(false);
    }

    public void ToggleSubPanelManual(bool _value)
    {
        myMenuOptions.SetActive(_value);
        GetComponent<Image>().sprite = myMenuOptions.activeInHierarchy ? selectedSprite : unselectedSprite;

        if (simulationBtn && myMenuOptions.activeInHierarchy)
        {
            FindObjectOfType<CameraController>().InitCamera();
        }
    }

    void ToggleSubPanelByEvent(GameObject _panelOpened)
    {
        // If this is the panel clicked, toggle it.
        if (_panelOpened == gameObject)
        {
            myMenuOptions.SetActive(!myMenuOptions.activeInHierarchy);
            if(simulationBtn && myMenuOptions.activeInHierarchy)
            {
                FindObjectOfType<CameraController>().InitCamera();
            }
        }
        else
        {
            // If not, disable it
            myMenuOptions.SetActive(false);
        }

        GetComponent<Image>().sprite = myMenuOptions.activeInHierarchy ? selectedSprite : unselectedSprite;
    }

    public void DisableButtons()
    {
        myToggle.interactable = false;

        CanvasGroup _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _canvasGroup.DOFade(.15f, 1);
    }

    public void EnableButtons()
    {
        myToggle.interactable = true;

        CanvasGroup _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        _canvasGroup.DOFade(1, 1);
    }
}