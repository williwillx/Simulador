using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class TypeFluid : MonoBehaviour
{
    [Header("Components")]
    public ObiParticleRenderer particleRenderer;
    public ObiFluidPropertyColorizer propertyColorizer;
    public ObiEmitter emitter;

    [Header("Boleans")]
    public bool canMixer = false;
    public bool isDreg = false;
    public float timeLerp = 0.01f;

    [Header("Colors")]
    public Color mixColor, mixDregs;

    
    [SerializeField]private Color startColor;
    [SerializeField] private Color startColorizer;
    void Start()
    {
        particleRenderer = GetComponent<ObiParticleRenderer>();
        propertyColorizer = GetComponent<ObiFluidPropertyColorizer>();
        emitter = GetComponent<ObiEmitter>();
        startColor = particleRenderer.particleColor;
        startColorizer = propertyColorizer.color;
    }

    public void StartVariable()
    {
        particleRenderer = GetComponent<ObiParticleRenderer>();
        propertyColorizer = GetComponent<ObiFluidPropertyColorizer>();
        emitter = GetComponent<ObiEmitter>();
        //startColor = particleRenderer.particleColor;
        //startColorizer = propertyColorizer.color;
        gameObject.SetActive(false);
    }

    public void MixFluids()
    {
        canMixer = true;
        propertyColorizer.enabled = true;

    }
    public void DisableMixFluids()
    {
        particleRenderer.particleColor = startColor;
        propertyColorizer.color = startColorizer;
        canMixer = false;
        propertyColorizer.enabled = false;

    }
    public void KillParticle()
    {

        StartCoroutine(DecreaseScaleParticle());

    }

    IEnumerator DecreaseScaleParticle()
    {
        while (particleRenderer.radiusScale > 0)
        {
            yield return new WaitForSeconds(0.1f);
            particleRenderer.radiusScale -= 0.05f;
        }
        //gameObject.SetActive(false);

    }
    void Update()
    {
        if (canMixer)
        {
            if (!isDreg)
            {
                particleRenderer.particleColor = Color.Lerp(particleRenderer.particleColor, mixColor, timeLerp);
                propertyColorizer.color = Color.Lerp(propertyColorizer.color, mixColor, timeLerp);
            }
            else
            {
                particleRenderer.particleColor = Color.Lerp(particleRenderer.particleColor, mixDregs, timeLerp);
                propertyColorizer.color = Color.Lerp(propertyColorizer.color, mixDregs, timeLerp);
            }
        }
    }
}
