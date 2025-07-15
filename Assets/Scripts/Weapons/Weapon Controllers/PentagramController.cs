using UnityEngine;

public class PentagramController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedPentagram = Instantiate(weaponData.Prefab);
        spawnedPentagram.transform.position = transform.position;
        spawnedPentagram.transform.parent = transform;
    }
}
