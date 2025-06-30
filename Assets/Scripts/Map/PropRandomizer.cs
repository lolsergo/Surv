using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;

    void Start()
    {
        SpawnProps();
    }

    void Update()
    {

    }

    void SpawnProps()
    {
        foreach (GameObject spawnProp in propSpawnPoints)
        {
            int rand = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[rand], spawnProp.transform.position, Quaternion.identity);
            prop.transform.parent = spawnProp.transform;
        }
    }
}
