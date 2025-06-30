using UnityEngine;

public class ThrowingSpearBehaviour : ProjectileWeaponBehaviour
{
    ThrowingSpearController throwingSpearController;

    protected override void Start()
    {
        base.Start();
        throwingSpearController = FindObjectOfType<ThrowingSpearController>();
    }

    void Update()
    {
        transform.position += direction * throwingSpearController.speed * Time.deltaTime;
    }
}
