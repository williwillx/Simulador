using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransparencyChild : MonoBehaviour
{
    Material[] originalMaterials;
    //[SerializeField] Material[] opaqueMaterials;
    [SerializeField] Material[] fadeMaterials;
    TransparencyParent transparencyParent;

    [Header("Tweaks")]
    float transparencyEnablingTime;
    float transparencyDisablingTime;
    float transparencyValue = 0.1f;

    int originalLayer;

    private void Start()
    {
        originalMaterials = GetComponent<MeshRenderer>().materials;
       // opaqueMaterials = new Material[originalMaterials.Length];
        fadeMaterials = new Material[originalMaterials.Length];

        GetTransparencyParent(transform.parent);
        //SetOpaqueMaterials();
        SetFadeMaterials();
        GetTweakValues();

        originalLayer = gameObject.layer;
    }

    //private void SetOpaqueMaterials()
    //{
    //    for (int i = 0; i < originalMaterials.Length; i++)
    //    {
    //        Material _opaqueMaterial = transparencyParent.opaqueMaterial;
    //        opaqueMaterials[i] = (Material)Instantiate(_opaqueMaterial);

    //        opaqueMaterials[i].SetTexture("_MainTex", originalMaterials[i].GetTexture("_MainTex"));
    //        opaqueMaterials[i].SetTexture("_OcclusionMap", originalMaterials[i].GetTexture("_OcclusionMap"));
    //        opaqueMaterials[i].SetTexture("_BumpMap", originalMaterials[i].GetTexture("_BumpMap"));

    //        opaqueMaterials[i].SetFloat("_BumpScale", originalMaterials[i].GetFloat("_BumpScale"));
    //        opaqueMaterials[i].SetFloat("_Metallic", originalMaterials[i].GetFloat("_Metallic"));
    //        opaqueMaterials[i].SetFloat("_OcclusionStrength", originalMaterials[i].GetFloat("_OcclusionStrength"));
    //        opaqueMaterials[i].SetFloat("_Glossiness", originalMaterials[i].GetFloat("_Glossiness"));
    //    }
    //}

    private void SetFadeMaterials()
    {
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            Material _fadeMaterial = transparencyParent.fadeMaterial;
            fadeMaterials[i] = (Material)Instantiate(_fadeMaterial);

            fadeMaterials[i].SetTexture("_MainTex", originalMaterials[i].GetTexture("_MainTex"));
            fadeMaterials[i].SetTexture("_OcclusionMap", originalMaterials[i].GetTexture("_OcclusionMap"));
            fadeMaterials[i].SetTexture("_BumpMap", originalMaterials[i].GetTexture("_BumpMap"));

            fadeMaterials[i].SetFloat("_BumpScale", originalMaterials[i].GetFloat("_BumpScale"));
            fadeMaterials[i].SetFloat("_Metallic", originalMaterials[i].GetFloat("_Metallic"));
            fadeMaterials[i].SetFloat("_OcclusionStrength", originalMaterials[i].GetFloat("_OcclusionStrength"));
            fadeMaterials[i].SetFloat("_Glossiness", originalMaterials[i].GetFloat("_Glossiness"));
        }
    }

    void GetTweakValues()
    {
        transparencyEnablingTime = transparencyParent.transparencyEnablingTime;
        transparencyDisablingTime = transparencyParent.transparencyDisablingTime;
        transparencyValue = transparencyParent.transparencyValue;
    }

    void GetTransparencyParent(Transform _objToGetComponent)
    {
        if (GetComponent<MeshRenderer>() == null) return;

        transparencyParent = _objToGetComponent.GetComponent<TransparencyParent>();
        if (transparencyParent == null)
        {
            GetTransparencyParent(_objToGetComponent.parent);
        }
    }

    public void EnableTransparency()
    {
        if (GetComponent<MeshRenderer>() == null) return;
        GetComponent<MeshRenderer>().materials = fadeMaterials;

        for (int i = 0; i < fadeMaterials.Length; i++)
        {
            // fadeMaterials[i].DOFade(transparencyValue, transparencyEnablingTime);
            foreach (var mat in fadeMaterials)
            {
                Color tempColor = mat.color;
                tempColor.a = transparencyValue;
                mat.color = tempColor;
            }
        }

        gameObject.layer = 0;
    }

    public void DisableTransparency()
    {
        if (GetComponent<MeshRenderer>() == null) return;

        for (int i = 0; i < fadeMaterials.Length; i++)
        {
            GetComponent<MeshRenderer>().materials = originalMaterials;

            //fadeMaterials[i].DOFade(1, transparencyDisablingTime).OnComplete(() => 
            //{
            //    GetComponent<MeshRenderer>().materials = originalMaterials;
            //});
        }

        gameObject.layer = originalLayer;
    }
}
