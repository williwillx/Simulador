using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FuelOptimizerController : MonoBehaviour
{
    [Header("StepOne")]
    [SerializeField] private Transform bocaCombustivel;
    [SerializeField] private Transform tampaCombustivel;
    [SerializeField] private Transform fuelOptmizer;
    [SerializeField] private float DurationAnim=2f;
    private Quaternion startTampaRot;
    private Quaternion startBocaRot;
    private Vector3 startTampaPos;
    private Vector3 startBocaPos;

    [Header("StepTwo")]
    [SerializeField] private ObjectsSimulation objectsSimulation;

    

    [Header("StepFour")]
    [SerializeField] private ChangeColorParticle particleSmoke;

    private void Awake()
    {
        DOTween.Init();
    }
    void Start()
    {
        startTampaRot=tampaCombustivel.localRotation;
        startTampaPos=tampaCombustivel.localPosition;

        startBocaRot = bocaCombustivel.localRotation;
        startBocaPos = bocaCombustivel.localPosition;
        objectsSimulation = GetComponent<ObjectsSimulation>();
        particleSmoke.gameObject. SetActive(false);
        fuelOptmizer.GetComponentInChildren<MeshRenderer>().material.DOFade(0, 0.1f);
        fuelOptmizer.gameObject.SetActive(false);
    }


    public void StepOne()
    {
        ResetStepOne();

        tampaCombustivel.DOLocalRotate(new Vector3(tampaCombustivel.rotation.x, -90f, tampaCombustivel.rotation.z), DurationAnim).SetDelay(1f);

        
        bocaCombustivel.DOLocalRotate(new Vector3( 360f, bocaCombustivel.rotation.y, bocaCombustivel.rotation.z), DurationAnim/2, RotateMode.FastBeyond360).SetLoops(2).SetRelative(true).SetEase(Ease.Linear);
        bocaCombustivel.DOLocalMoveX(0.01f,DurationAnim).SetDelay(2f);
        bocaCombustivel.GetComponentInChildren<MeshRenderer>().material.DOFade(0, DurationAnim).SetDelay(2f);

        fuelOptmizer.DOLocalRotate(new Vector3(fuelOptmizer.rotation.x, fuelOptmizer.rotation.y,107), DurationAnim).SetDelay(3.2f);
        fuelOptmizer.DOLocalMoveX(3.04f,DurationAnim).SetDelay(3f);
        fuelOptmizer.GetComponentInChildren<MeshRenderer>().material.DOFade(1, DurationAnim).SetDelay(2f);
        fuelOptmizer.gameObject.SetActive(true);

    }
    private void ResetStepOne()
    {
        objectsSimulation.DisableObjects();
        tampaCombustivel.localRotation = startTampaRot;
        tampaCombustivel.localPosition = startTampaPos;
        bocaCombustivel.localRotation = startBocaRot;
        bocaCombustivel.localPosition = startBocaPos;

        bocaCombustivel.GetComponentInChildren<MeshRenderer>().material.DOFade(1, 0.1f);

        fuelOptmizer.GetComponentInChildren<MeshRenderer>().material.DOFade(0, 0.1f);
        fuelOptmizer.DOLocalRotate(new Vector3(fuelOptmizer.rotation.x, fuelOptmizer.rotation.y, 0), 0.1f);
        fuelOptmizer.DOLocalMoveX(4, 0.1f);
    }
    public void StepTwo()
    {
        fuelOptmizer.DOLocalRotate(new Vector3(fuelOptmizer.rotation.x, fuelOptmizer.rotation.y, 107), 0.1f);
        fuelOptmizer.DOLocalMoveX(3.04f, 0.1f);
        objectsSimulation.StartInitialSimulation();
    }

    public void StepThree()
    {
        objectsSimulation.SetEnableOrDisablePathGasoline(true);
    }
    public void StepFour()
    {
        //objectsSimulation.SetEnableOrDisablePathGasoline(false);
        particleSmoke.gameObject.SetActive(true);
        particleSmoke.ChangeColor(5f);
    }
    public void StepFive()
    {
        //objectsSimulation.DisableObjects();

    }

   
}
