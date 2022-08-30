using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ChangeColorParticle : MonoBehaviour
{
    [SerializeField] private Material particleMaterial;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color desiredColor;
    [SerializeField] private float duration;

    private void Awake()
    {
        DOTween.Init();
    }
    void Start()
    {
        var main = gameObject.GetComponent<ParticleSystemRenderer>();
        particleMaterial = main.material;

        initialColor = particleMaterial.color;
    }

    private void OnEnable()
    {
        var main = gameObject.GetComponent<ParticleSystemRenderer>();
        particleMaterial = main.material;

        //initialColor = particleMaterial.color;
    }

    private void ResetColor()
    {
        particleMaterial.DOColor(initialColor, 0.1f);
        particleMaterial.DOColor(initialColor, "_EmissionColor", 0.1f);
    }
    public void ChangeColor(float delay=3f)
    {
        ResetColor();
        particleMaterial.DOColor(desiredColor,duration).SetDelay(delay);
        particleMaterial.DOColor(desiredColor,"_EmissionColor", duration).SetDelay(delay);
    }
    
}
