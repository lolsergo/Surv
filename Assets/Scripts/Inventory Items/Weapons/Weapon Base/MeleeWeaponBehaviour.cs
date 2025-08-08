using UnityEngine;

public class MeleeWeaponBehaviour : WeaponBehaviour
{
    public MeeleWeaponScriptableObject meleeWeaponData;

    protected override void Awake()
    {
        weaponData = meleeWeaponData;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
    }
}
