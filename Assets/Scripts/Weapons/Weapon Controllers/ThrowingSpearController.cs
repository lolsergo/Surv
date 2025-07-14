using UnityEngine;

public class ThrowingSpearController : WeaponController
{
    [SerializeField]
    private RangedWeaponScriptableObject rangedWeaponData;

    protected override void Start()
    {
        base.Start();
        weaponData = rangedWeaponData;
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSpear = Instantiate(rangedWeaponData.Prefab);
        spawnedSpear.transform.position = transform.position;
        spawnedSpear.GetComponent<ThrowingSpearBehaviour>().DirectionChecker(playerMovement.lastMovedVector);
    }
}
