using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceParticle : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    [SerializeField] private float spawnTime = 1;
    [SerializeField] private float startSpawn = 1;
    [SerializeField] private PathCreation.Examples.PathFollower pathFollower; 
    void Start()
    {
        pathFollower = GetComponent<PathCreation.Examples.PathFollower>();
    }

    private void OnEnable()
    {
        StartCoroutine(ICallSpawnParticle(startSpawn));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void ResetDistanceTravelled()
    {   
        pathFollower.distanceTravelled = 0;
    }
    private void SpawnParticle()
    {
        GameObject inst = Instantiate(particle, transform.position, Quaternion.identity);
        inst.transform.SetParent(transform);
        if(pathFollower.distanceTravelled<1.3f)
        StartCoroutine(ICallSpawnParticle(spawnTime));
    }

    IEnumerator ICallSpawnParticle(float _time)
    {
        yield return new WaitForSeconds(_time);
        SpawnParticle();
    }
}
