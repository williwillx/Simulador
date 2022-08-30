using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelAjudaOption : MonoBehaviour
{
    [SerializeField] string txtTitle;
    [SerializeField] GameObject myContentPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(()
            => UIManager.instance.ChangeHelpPanelTexts(txtTitle, myContentPanel));
    }
}
