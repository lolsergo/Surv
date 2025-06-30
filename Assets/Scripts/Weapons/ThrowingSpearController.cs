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
        GameObject spawnedSpear = Instantiate(prefab);
        spawnedSpear.transform.position = transform.position;
        spawnedSpear.GetComponent<ThrowingSpearBehaviour>().DirectionChecker(playerMovement.moveDirection);
    }
}
