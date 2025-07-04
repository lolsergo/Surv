using UnityEngine;

public class ThrowingSpearBehaviour : ProjectileWeaponBehaviour
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.position += direction * weaponData.FlightSpeed * Time.deltaTime;
    }
}
