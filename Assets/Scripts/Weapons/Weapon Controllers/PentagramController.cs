using UnityEngine;

public class PentagramController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedPentagram = Instantiate(prefab);
        spawnedPentagram.transform.position = transform.position;
        spawnedPentagram.transform.parent = transform;
    }
}
