using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuToggler : MonoBehaviour
{
    [SerializeField] GameObject mySubMenu;
    public bool isCurrentlySelected;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    Image myImg;

    private void OnEnable()
    {
        UIManager.OnSubMenuPanelOpened += ToggleSubMenu;
    }

    private void OnDisable()
    {
        UIManager.OnSubMenuPanelOpened -= ToggleSubMenu;
        mySubMenu.SetActive(false);
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UIManager.instance.OnSubMenuOpened(gameObject));
        myImg = GetComponent<Image>();
    }

    private void Update()
    {
        myImg.sprite = AtLeastOneSubMenuIsSelected() || isCurrentlySelected ? selectedSprite : unselectedSprite;        
    }

    bool AtLeastOneSubMenuIsSelected()
    {
        if (!mySubMenu.activeInHierarchy) return false;

        for (int i = 0; i < mySubMenu.transform.childCount; i++)
        {
            ActionUIButton actionButton = mySubMenu.transform.GetChild(i).GetComponent<ActionUIButton>();
            return true;
        }

        return false;
    }

    public void ToggleSubMenu(GameObject _menuOpened)
    {
        mySubMenu.SetActive(_menuOpened == gameObject);

        isCurrentlySelected = mySubMenu.activeInHierarchy;

        for (int i = 0; i < mySubMenu.transform.childCount; i++)
        {
            ActionUIButton actionButton = mySubMenu.transform.GetChild(i).GetComponent<ActionUIButton>();

            if (actionButton == null) continue;

            if (actionButton.changeToSelectedOnClick.Count == 0)
            {
                actionButton.changeToSelectedOnClick.Add(GetComponent<Image>());
                actionButton.changeToSelectedOnClick.Add(transform.parent.parent.GetComponent<Image>());
                actionButton.mySubMenuToggler = this;
            }

            actionButton.isCurrentlySelected = false;
        }
    }
}