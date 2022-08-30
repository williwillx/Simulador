using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioAirController : MonoBehaviour
{
    [Header("StepTwo")]
    [SerializeField] private GameObject bioAir;
    [SerializeField] private InstanceParticle followerLeft;
    [SerializeField] private InstanceParticle followerRight;
    [SerializeField] private GameObject sparkEffect, sparkEffectSmall;
    [SerializeField] private SmokeAerosolSettings smokeAerosol;
    [SerializeField] private Material flammable;
    [SerializeField] private Material notFlammable;

    [Header("StepThree")]
    [SerializeField] private SmokeAerosolSettings smokeFull;
    [SerializeField] private SmokeAerosolSettings smokeFullSmall;

    [Header("StepSix")]
    [SerializeField] private Light carLight;
    [SerializeField] private float delayActiveLight = 3f;
    [SerializeField] private float intensityLight = 100f;
    [SerializeField] private float speedLight = 2f;

    [Space]
    [SerializeField] private List<GameObject> glass;
    private List<Vector3> glassPositions = new List<Vector3>();
    [SerializeField] private GameObject glassDoorRight, glassOriginal, glassBrick;



    [Header("Doors Objects")]

    [SerializeField] private List<CarDoor> doors;

    void Start()
    {
        bioAir.SetActive(false);
        smokeFull.gameObject.SetActive(false);
        smokeFullSmall.gameObject.SetActive(false);
        followerLeft.gameObject.SetActive(false);
        followerRight.gameObject.SetActive(false);
        carLight.gameObject.SetActive(false);
        if (glassPositions.Count > 0)
            glassPositions.Clear();

        for (int i = 0; i < glass.Count; i++)
        {
            glassPositions.Add(glass[i].transform.position);
        }
       
    }


    public void StepOne()
    {
        bioAir.SetActive(false);
        DisableGlass();
        // chamar função de fechar porta
        foreach (var item in doors)
        {
            item.OpenDoor(0.1f);
        }

        foreach (var item in doors)
        {
            item.CloseDoor(1.5f, 3f);
        }
    }

    public void ChangeTexture(bool isFlammable)
    {
        List<MeshRenderer> childrens = new List<MeshRenderer>();
        childrens.AddRange(bioAir.GetComponentsInChildren<MeshRenderer>());
        for (int i = 0; i < childrens.Count; i++)
        {
            if (isFlammable)
            {

                childrens[i].material=flammable;
            }
            else
            {
                childrens[i].material=notFlammable;
            }
        }


    }
    public void StepTwo()
    {
        bioAir.SetActive(true);
        foreach (var item in doors)
        {
            item.CloseDoor(0.1f);
        }
        smokeAerosol.gameObject.SetActive(true);
        smokeAerosol.ActiveLoop();
        smokeAerosol.DisactivePrewarm();
        followerLeft.gameObject.SetActive(false);
        followerRight.gameObject.SetActive(false);
        sparkEffect.SetActive(false);
        sparkEffectSmall.SetActive(false);
        smokeFull.gameObject.SetActive(false);
        smokeFullSmall.gameObject.SetActive(false);
        CancelInvoke("ActivePath");
        CancelInvoke("ActiveSmokeFull");
    }
    public void StepThree()
    {
        smokeFull.DisactivePrewarm();
        smokeFullSmall.DisactivePrewarm();
        smokeAerosol.ActivePrewarm();
        DisableSmokeFull();
        foreach (var item in doors)
        {
            item.CloseDoor(0.1f);
        }
        smokeAerosol.ActiveDisableLoop(3f);
        ResetGlassPosition();
        DisableGlass();
        Invoke("ActivePath", 2f);
        Invoke("ActiveSmokeFull", 4f);
    }

    private void ActivePath()
    {
        sparkEffect.SetActive(true);
        sparkEffectSmall.SetActive(true);
        followerLeft.gameObject.SetActive(true);
        followerRight.gameObject.SetActive(true);
        followerLeft.ResetDistanceTravelled();
        followerRight.ResetDistanceTravelled();
        DisableSmokeFull();
    }
    private void DesactivePath()
    {
        followerLeft.gameObject.SetActive(false);
        followerRight.gameObject.SetActive(false);
        sparkEffect.SetActive(false);
        sparkEffectSmall.SetActive(false);
    }
    private void ActiveSmokeFull()
    {
        CancelInvoke("DisableSmokeFull");
        smokeFull.gameObject.SetActive(true);
        smokeFullSmall.gameObject.SetActive(true);
    }

    private void DisableSmokeFull()
    {
        smokeFull.ActiveLoop();
        smokeFullSmall.ActiveLoop();
        smokeFull.gameObject.SetActive(false);
        smokeFullSmall.gameObject.SetActive(false);
    }
    public void StepFour()
    {
        foreach (var item in doors)
        {
            item.CloseDoor(0.1f);
        }
        DisableSmokeFull();
        ActiveSmokeFull();
        smokeFull.ActivePrewarm();
        smokeFullSmall.ActivePrewarm();
        smokeFull.ActiveDisableLoop();
        smokeFullSmall.ActiveDisableLoop();
        DesactivePath();

        foreach (var item in doors)
        {
            item.OpenDoor(2f, 2f);
        }
    }

    public void StepFive()
    {
        followerLeft.gameObject.SetActive(false);
        followerRight.gameObject.SetActive(false);
        DisableSmokeFull();
        StopAllCoroutines();
        carLight.intensity = 3;
        foreach (var item in doors)
        {
            item.CloseDoor(1.5f, 3f);
        }
    }

    IEnumerator BlinkLight(float time)
    {
        carLight.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);
        while (carLight.intensity < intensityLight)
        {
            yield return new WaitForSeconds(0.001f);
            carLight.intensity += speedLight;
        }
        AudioManager.instance.PlayAudioGlassBrick();
        BreakGlass();
        DisableSmokeFull();
        yield return new WaitForSeconds(0.1f);
        while (carLight.intensity > 6)
        {
            yield return new WaitForSeconds(0.001f);
            carLight.intensity -= speedLight;
        }
    }

    private void DisableGlass()
    {
        glassDoorRight.SetActive(true);
        glassOriginal.SetActive(true);
        foreach (var item in glass)
        {
            item.SetActive(false);
        }
        glassBrick.SetActive(false);
    }
    private void ResetGlassPosition()
    {
        for (int i = 0; i < glass.Count; i++)
        {
            glass[i].transform.position = glassPositions[i];
        }
    }
    private void BreakGlass()
    {
       
        glassDoorRight.SetActive(false);
        glassOriginal.SetActive(false);
        glassBrick.SetActive(true);

        for (int i = 0; i < glass.Count; i++)
        {
            glass[i].SetActive(true);
            if (i > 0)
            {
                Rigidbody bodyItem = glass[i].GetComponent<Rigidbody>();
                bodyItem.AddForce(new Vector3(0, 5, 0));
                bodyItem.isKinematic = false;
            }
        }

    }
    public void StepSix()
    {
        foreach (var item in doors)
        {
            item.CloseDoor(0.1f);
        }
        DisableSmokeFull();
        ActiveSmokeFull();
        smokeFull.ActivePrewarm();
        smokeFullSmall.ActivePrewarm();
        smokeFull.ActiveDisableLoop();
        smokeFullSmall.ActiveDisableLoop();
        DesactivePath();
        StartCoroutine(BlinkLight(delayActiveLight));
    }

}
