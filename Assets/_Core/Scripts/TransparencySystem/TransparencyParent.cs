using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyParent : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] bool shouldTakeAllMeshs;
    [SerializeField] List<GameObject> transparentGameobjects = new List<GameObject>();

    [Header("Materials")]
    public Material fadeMaterial;
    public Material opaqueMaterial;

    [Header("Tweaks")]
    public float transparencyEnablingTime;
    public float transparencyDisablingTime;

    [Range(0, 1)]
    public float transparencyValue = 0.1f;

    private void Start()
    {
        GetTransparencyChildList(transform);
    }

    public void EnableTransparency()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Transparency works only in playing mode");
            return;
        }

        foreach (var go in transparentGameobjects)
        {
            go.GetComponent<TransparencyChild>().EnableTransparency();
            go.layer = 0;
        }
    }

    public void DisableTransparency()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Transparency works only in playing mode");
            return;
        }

        foreach (var go in transparentGameobjects)
        {
            go.GetComponent<TransparencyChild>().DisableTransparency();
           // StartCoroutine(DisableTransparencyCor(mat));
        }
    }

    IEnumerator EnableTransparencyCor(GameObject go, int _materialIndex)
    {
        print("Enabling transparency to " + go.name + " objects");
        Material _thisMaterial = go.GetComponent<MeshRenderer>().materials[_materialIndex];
        Material _fadeMaterial = (Material)Instantiate(fadeMaterial);

        go.GetComponent<MeshRenderer>().materials[_materialIndex] = _fadeMaterial;

        _fadeMaterial.SetTexture("_MainTex", _thisMaterial.GetTexture("_MainTex"));
        _fadeMaterial.SetTexture("_OcclusionMap", _thisMaterial.GetTexture("_OcclusionMap"));
        _fadeMaterial.SetTexture("_BumpMap", _thisMaterial.GetTexture("_BumpMap"));

        _fadeMaterial.SetFloat("_BumpScale", _thisMaterial.GetFloat("_BumpScale"));
        _fadeMaterial.SetFloat("_Metallic", _thisMaterial.GetFloat("_Metallic"));
        _fadeMaterial.SetFloat("_OcclusionStrength", _thisMaterial.GetFloat("_OcclusionStrength"));
        _fadeMaterial.SetFloat("_Glossiness", _thisMaterial.GetFloat("_Glossiness"));

        Color tempColor = _fadeMaterial.color;

        for (float f = transparencyEnablingTime; f >= transparencyValue; f -= 0.1f)
        {
            tempColor.a = f;
            _fadeMaterial.color = tempColor;
            yield return null;
        }
    }

    IEnumerator DisableTransparencyCor(GameObject go)
    {
        Material tempMaterial = go.GetComponent<MeshRenderer>().material;

        Color tempColor = tempMaterial.color;

        for (float f = transparencyValue; f <= transparencyDisablingTime; f += 0.1f)
        {
            tempColor.a = f;
            tempMaterial.color = tempColor;
            yield return null;
        }

        Material _opaqueMaterial = (Material)Instantiate(opaqueMaterial);
        go.GetComponent<MeshRenderer>().material = _opaqueMaterial;

        _opaqueMaterial.SetTexture("_MainTex", tempMaterial.GetTexture("_MainTex"));
        _opaqueMaterial.SetTexture("_OcclusionMap", tempMaterial.GetTexture("_OcclusionMap"));
        _opaqueMaterial.SetTexture("_BumpMap", tempMaterial.GetTexture("_BumpMap"));

        _opaqueMaterial.SetFloat("_BumpScale", tempMaterial.GetFloat("_BumpScale"));
        _opaqueMaterial.SetFloat("_Metallic", tempMaterial.GetFloat("_Metallic"));
        _opaqueMaterial.SetFloat("_OcclusionStrength", tempMaterial.GetFloat("_OcclusionStrength"));
        _opaqueMaterial.SetFloat("_Glossiness", tempMaterial.GetFloat("_Glossiness"));
        _opaqueMaterial.EnableKeyword("_NORMALMAP");
    }

    void GetTransparencyChildList(Transform _objBeingIterated)
    {
        if (_objBeingIterated.childCount <= 0) return;

        for (int i = 0; i < _objBeingIterated.childCount; i++)
        {
            // Change the target object if every mesh should be transparent
            if (shouldTakeAllMeshs)
            {
                MeshRenderer[] childrenWithTransparency = GetComponentsInChildren<MeshRenderer>();
                AddChildrenToTransparencyList(childrenWithTransparency);
            }
            else
            {
                TransparencyChild[] childrenWithTransparency = GetComponentsInChildren<TransparencyChild>();
                AddChildrenToTransparencyList(childrenWithTransparency);
            }

            // If this object has children, search for it's children as well
            if (_objBeingIterated.GetChild(i))
            {
                GetTransparencyChildList(_objBeingIterated.GetChild(i));
            }
        }
    }

    private void AddChildrenToTransparencyList(MeshRenderer[] childrenWithTransparency)
    {
        if (childrenWithTransparency.Length > 0)
        {
            foreach (var child in childrenWithTransparency)
            {
                // Guarantee that this object is not being added twice
                if (!transparentGameobjects.Contains(child.gameObject))
                {
                    transparentGameobjects.Add(child.gameObject);
                    child.gameObject.AddComponent<TransparencyChild>();
                }
            }
        }
    }

    private void AddChildrenToTransparencyList(TransparencyChild[] childrenWithTransparency)
    {
        if (childrenWithTransparency.Length > 0)
        {
            foreach (var child in childrenWithTransparency)
            {
                // Guarantee that this object is not being added twice
                if (!transparentGameobjects.Contains(child.gameObject))
                {
                    transparentGameobjects.Add(child.gameObject);
                }
            }
        }
    }
}