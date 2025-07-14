using UnityEngine;

public class PentagramController : WeaponController
{
    [SerializeField]
    private MeeleWeaponScriptableObject meleeWeaponData;

    protected override void Start()
    {
        base.Start();
        weaponData = meleeWeaponData;
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedPentagram = Instantiate(meleeWeaponData.Prefab);
        spawnedPentagram.transform.position = transform.position;
        spawnedPentagram.transform.parent = transform;
    }
}
