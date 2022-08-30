using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
public class ObjectsSimulation : MonoBehaviour
{
    public TypeFluid gasoline, water, water2, dregs, dregs2, dregs3;
    [SerializeField] private SplineMesh.ExampleContortAlong path, pathGasoline;
    public float timeGasoline, timeDregs, timeWater, timeOptimizer, timeMix, timeGeral, newAbsorption, startAborption;
    public GameObject solverPrincipal;
    public ObiFluidRenderer fluidRenderer;
    public bool canOpacity;
    void Start()
    {
        Invoke("DisableObjects",1f);
        startAborption = fluidRenderer.settings.absorption;
    }

    public void DisableObjects()
    {
        StopAllCoroutines();

        gasoline.StartVariable();
        water.StartVariable();
        water2.StartVariable();
        dregs.StartVariable();
        dregs2.StartVariable();
        dregs3.StartVariable();
        path.rate = 0;
        pathGasoline.rate = 0;
        path.gameObject.SetActive(false);
        pathGasoline.gameObject.SetActive(false);
    }
    public void StartInitialSimulation()
    {
        gasoline.DisableMixFluids();
        water.DisableMixFluids();
        water2.DisableMixFluids();
        dregs.DisableMixFluids();
        dregs2.DisableMixFluids();
        dregs3.DisableMixFluids();
        DisableObjects();
        canOpacity = false;
        fluidRenderer.settings.absorption = startAborption;
        StopAllCoroutines();
        StartCoroutine(InitialSimultion());
    }

    private void ResetSimulation()
    {

    }
    private IEnumerator InitialSimultion()
    {
        yield return new WaitForSeconds(timeGasoline);
        gasoline.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeDregs);
        dregs.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeDregs);
        dregs2.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeDregs);
        dregs3.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeWater);
        water.gameObject.SetActive(true);
        water2.gameObject.SetActive(true);
        StartOptimizerSimulation();
    }
    public void StartOptimizerSimulation()
    {
        StartCoroutine(OptimizerSimulation());
    }
    private IEnumerator OptimizerSimulation()
    {
        yield return new WaitForSeconds(timeOptimizer);
        path.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeMix);
        gasoline.MixFluids();
        water.MixFluids();
        water2.MixFluids();
        dregs.MixFluids();
        dregs2.MixFluids();
        dregs3.MixFluids();
        yield return new WaitForSeconds(timeGasoline);
        DecreaseOpacity();
        yield return new WaitForSeconds(timeGeral);
    }

    public void DecreaseOpacity()
    {
        canOpacity = true;
    }
    public void SetEnableOrDisablePathGasoline(bool state)
    {
        pathGasoline.gameObject.SetActive(state);
    }
    void Update()
    {
        if (canOpacity)
        {
            fluidRenderer.settings.absorption = Mathf.Lerp(fluidRenderer.settings.absorption, newAbsorption, 0.01f);
        }
        if (path.rate >= 0.96f)
        {
            path.gameObject.SetActive(false);
        }

        //if (pathGasoline.rate >= 0.96f)
        //{
        //    pathGasoline.gameObject.SetActive(false);
        //}

    }

}
