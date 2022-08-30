using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum ActionButtonMode
{
    CameraMovement,
    BioAirSimulation
}

public class ActionUIButton : MonoBehaviour
{
    [Header("Camera Movement Mode")]
    [SerializeField] bool shouldMoveCamera = true;
    [SerializeField] Transform moveCameraTo;
    [SerializeField] Transform lookAtTarget;
    [SerializeField] float minCameraDistance = 1;
    [SerializeField] float maxCameraDistance = 4;

    [Header("Lights config")]
    [SerializeField] bool shouldToggleLights = false;
    [SerializeField] GameObject lightsGO;

    [Header("Transparency config")]
    [SerializeField] bool shouldChangeVisibility;
    [SerializeField] bool makeEverythingInvisible = true;
    TransparencyParent[] allParentsInScene;
    [SerializeField] TransparencyParent[] parentsToMakeInvisible;
    [SerializeField] TransparencyParent[] parentsToMakeVisible;
    [SerializeField] GameObject[] childrenToMakeInvisible;
    [SerializeField] GameObject[] childrenToMakeVisible;

    [Header("UI config")]
    public List<Image> changeToSelectedOnClick = new List<Image>();

     public bool isCurrentlySelected;
    public SubMenuToggler mySubMenuToggler;
    Image myImg;

    private void OnEnable()
    {
        UIManager.OnStartButtonPushed += DeactiveParent;
    }

    private void OnDisable()
    {
        UIManager.OnStartButtonPushed -= DeactiveParent;
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (shouldMoveCamera)
            GetComponent<Button>().onClick.AddListener(MoveCameraToTransform);

        if (shouldToggleLights)
            GetComponent<Button>().onClick.AddListener(ToogleLights);

        if (shouldChangeVisibility)
            GetComponent<Button>().onClick.AddListener(ChangeVisibility);

        GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeParentsToSelected();
        });

        allParentsInScene = FindObjectsOfType<TransparencyParent>();

        myImg = GetComponent<Image>();
    }

    void DeactiveParent()
    {
        if (transform.parent.parent.name == "SubMenus")
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void ActiveUIActionButtonViaScript()
    {
        MoveCameraToTransform();
        ToogleLights();
        ChangeVisibility();

        ChangeParentsToSelected();
        ManualSetSelectedButton(true);
    }

    public void DeactiveUIActionButtonViaScript()
    {
        ManualSetSelectedButton(false);
    }

    void ManualSetSelectedButton(bool _value)
    {
        if (mySubMenuToggler)
        {
            if (_value)
            {
                mySubMenuToggler.isCurrentlySelected = true;
                isCurrentlySelected = true;
                myImg.sprite = mySubMenuToggler.selectedSprite;
            }
            else
            {
                try
                {
                    mySubMenuToggler.isCurrentlySelected = false;
                    isCurrentlySelected = false;
                    myImg.sprite = mySubMenuToggler.unselectedSprite;
                } catch { }                
            }
        }
        else
        {
            if (_value)
            {
                SubMenuToggler menuToggler = GetComponent<SubMenuToggler>();
                if (menuToggler)
                {
                    menuToggler.isCurrentlySelected = true;
                    myImg.sprite = menuToggler.selectedSprite;
                }
            }
            else
            {
                SubMenuToggler menuToggler = GetComponent<SubMenuToggler>();
                if (menuToggler)
                {
                    menuToggler.isCurrentlySelected = false;
                    myImg.sprite = menuToggler.unselectedSprite;
                }
            }
        }
    }

    private void Update()
    {
        if (!mySubMenuToggler) return;

        myImg.sprite = isCurrentlySelected ? mySubMenuToggler.selectedSprite : mySubMenuToggler.unselectedSprite;
    }

    void MoveCameraToTransform()
    {
        if (!shouldMoveCamera) return;

        GameObject cameraGO = Camera.main.gameObject;
        CameraController _cameraController = cameraGO.GetComponent<CameraController>();

        _cameraController.MoveCameraTo(moveCameraTo, lookAtTarget, minCameraDistance, maxCameraDistance);
    }

    void ToogleLights()
    {
        if (!shouldToggleLights) return;
        lightsGO.SetActive(!lightsGO.activeInHierarchy);
    }

    void ChangeVisibility()
    {
        if (!shouldChangeVisibility) return;

        if (allParentsInScene.Length == 0)
        {
            allParentsInScene = FindObjectsOfType<TransparencyParent>();
        }

        if (makeEverythingInvisible)
        {
            foreach (var item in allParentsInScene)
            {
                item.EnableTransparency();
            }
        }
        else
        {
            foreach (var vParent in parentsToMakeInvisible)
            {
                vParent.EnableTransparency();
            }
        }

        foreach (var vParent in parentsToMakeVisible)
        {
            vParent.DisableTransparency();
        }

        foreach (var vChild in childrenToMakeInvisible)
        {
            if (vChild.GetComponent<TransparencyChild>() == null) continue;
            if (vChild.GetComponent<MeshRenderer>() == null) continue;
            vChild.GetComponent<TransparencyChild>().EnableTransparency();
        }

        foreach (var vChild in childrenToMakeVisible)
        {
            if (vChild.GetComponent<TransparencyChild>() == null) continue;
            if (vChild.GetComponent<MeshRenderer>() == null) continue;
            vChild.GetComponent<TransparencyChild>().DisableTransparency();
        }
    }

    void DeselectedAllNeighbors()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            var actionButton = transform.parent.GetChild(i).GetComponent<ActionUIButton>();
            if (actionButton)
            {
                actionButton.isCurrentlySelected = false;
                if (actionButton.mySubMenuToggler)
                {
                    actionButton.mySubMenuToggler.isCurrentlySelected = false;
                }
            }
        }
    }

    void ChangeParentsToSelected()
    {
        DeselectedAllNeighbors();

        transform.parent.gameObject.SetActive(true);
        foreach (var thisButton in changeToSelectedOnClick)
        {
            Button buttonComponent = thisButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                thisButton.sprite = buttonComponent.spriteState.selectedSprite;
                isCurrentlySelected = true;
            }

            Toggle toggleComponent = thisButton.GetComponent<Toggle>();
            if (toggleComponent != null)
            {
                thisButton.sprite = toggleComponent.spriteState.selectedSprite;
                isCurrentlySelected = true;
            }

            if (mySubMenuToggler)
            {
                mySubMenuToggler.isCurrentlySelected = true;
            }
        }
    }
}