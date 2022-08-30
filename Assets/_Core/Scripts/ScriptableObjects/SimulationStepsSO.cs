using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Simulation Step List", menuName = "ScriptableObjects/Simulation Step List")]
public class SimulationStepsSO : ScriptableObject
{
    public string productName;
    public SimulationStep[] simulationSteps;
}