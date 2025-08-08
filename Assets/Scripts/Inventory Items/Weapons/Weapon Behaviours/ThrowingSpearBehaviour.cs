using UnityEngine;

public class ThrowingSpearBehaviour : ProjectileWeaponBehaviour
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        transform.position += direction * currentProjectileSpeed * Time.deltaTime;
    }
}
