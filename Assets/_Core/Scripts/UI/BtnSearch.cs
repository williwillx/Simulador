using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BtnSearch : MonoBehaviour
{
    [SerializeField] InputField myInputField;
    [SerializeField] GameObject[] actionUIButtons;
    [SerializeField] GameObject[] childMenusActionsUIButtons;
    [SerializeField] SubMenuToggler[] AllSubTogglers;
    GameObject currentSelectedButton;
    string lastTextSearched = "";
    int lastIndexSearched = -1;

    void DisableAllSubTogglers()
    {
        foreach (var toggler in AllSubTogglers)
        {
            toggler.GetComponent<Image>().sprite = toggler.unselectedSprite;
            toggler.isCurrentlySelected = false;
        }
    }

    public void ResetVariablesIfTextIsEmpty()
    {
        string currentText = RemoveAccents(myInputField.text).ToLower();
        currentText = RemoveSpaces(currentText);

        if (currentText == "")
        {
            lastTextSearched = "";
            lastIndexSearched = 0;
        }
    }

    public void SearchActionButton()
    {
        string currentText = RemoveAccents(myInputField.text).ToLower();
        currentText = RemoveSpaces(currentText);

        DeactiveAllSubMenuUIActionButtons();

        bool hasAlreadyChanged = false;
        DisableAllSubTogglers();

        if (currentText == "")
        {
            lastTextSearched = "";
            lastIndexSearched = 0;
            return;
        }

        if (currentText != lastTextSearched)
        {
            lastIndexSearched = -1;
        } else
        {
            lastIndexSearched--;
        }

        int indexValue = currentText == lastTextSearched ? lastIndexSearched : 0;

        for (int i = indexValue; i < actionUIButtons.Length; i++)
        {
            if (i <= -1)
            {
                i = 0;
            }

            GameObject btn = actionUIButtons[i];
            if (btn.name.ToLower().Contains(currentText))
            {
                if (hasAlreadyChanged) continue;
                if (i == lastIndexSearched) continue;

                btn.GetComponent<ActionUIButton>().ActiveUIActionButtonViaScript();
                lastIndexSearched = i + 1;
                hasAlreadyChanged = true;
            }
            else
            {
                btn.GetComponent<ActionUIButton>().DeactiveUIActionButtonViaScript();
            }

            lastTextSearched = currentText;
            if (!hasAlreadyChanged)
            {
                lastIndexSearched = -1;
            }
        }
    }

    public string RemoveAccents(string text)
    {
        StringBuilder sbReturn = new StringBuilder();
        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }

    string RemoveSpaces(string _text)
    {
        return _text.Replace(" ", "");
    }

    void DeactiveAllSubMenuUIActionButtons()
    {
        foreach (var panel in childMenusActionsUIButtons)
        {
            panel.SetActive(false);
        }
    }
}
