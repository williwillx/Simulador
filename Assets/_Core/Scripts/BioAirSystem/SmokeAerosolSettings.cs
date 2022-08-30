using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAerosolSettings : MonoBehaviour
{
    public ParticleSystem particleSmoke;
    public float delay =4.5f;
    void Start()
    {
        particleSmoke = GetComponent<ParticleSystem>();
    }

    public void ActiveDisableLoop(float time=2f)
    {
        Invoke("DisableLoop", time);
    }

    public void ActiveLoop()
    {
        if (!particleSmoke)
        {
            particleSmoke = GetComponent<ParticleSystem>();
        }

        CancelInvoke("DisableLoop");
        gameObject.SetActive(true);
        particleSmoke.Play();
        particleSmoke.loop = true;
        particleSmoke.startDelay = delay;
    }

    private void DisableLoop()
    {
        particleSmoke.loop = false;
    }

    public void ActivePrewarm()
    {
        if (!particleSmoke)
        {
            particleSmoke = GetComponent<ParticleSystem>();
        }

        var main = particleSmoke.main;
        main.prewarm = true;
        particleSmoke.startDelay = 0;
    }

    public void DisactivePrewarm()
    {
        if (!particleSmoke)
        {
            particleSmoke = GetComponent<ParticleSystem>();
        }

        var main = particleSmoke.main;
        main.prewarm = false;
        particleSmoke.startDelay = delay;
    }
}