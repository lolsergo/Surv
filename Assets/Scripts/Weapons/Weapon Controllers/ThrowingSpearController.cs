using UnityEngine;

public class ThrowingSpearController : WeaponController
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSpear = Instantiate(weaponData.Prefab);
        spawnedSpear.transform.position = transform.position;
        spawnedSpear.GetComponent<ThrowingSpearBehaviour>().DirectionChecker(playerMovement.lastMovedVector);
    }
}
