using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSimulation : MonoBehaviour
{
    [SerializeField] SimulationStep[] simulationSteps;

    [Header("Text Config")]
    [SerializeField] Text myText;
    [SerializeField] Color textStandardColor;
    [SerializeField] Color textOverColor;
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<SimulationManager>().StartSimulation(simulationSteps);
        });
    }

    /// <summary>
    /// This method shouldn't exist and is here because Text component can't change it's color via Unity events
    /// </summary>
    public void SetOverColor()
    {
        myText.color = textOverColor;
    }

    /// <summary>
    /// This method shouldn't exist and is here because Text component can't change it's color via Unity events
    /// </summary>
    public void SetStandardColor()
    {
        myText.color = textStandardColor;
    }
}
