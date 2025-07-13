using UnityEngine;

public class ProjectileWeaponBehaviour : WeaponBehaviour
{
    protected Vector3 direction;
    public bool applyRotation = true;
    public float angleOffset = 0f;

    [SerializeField]
    protected float currentProjectileSpeed;
    [SerializeField]
    protected int currentPierce;

    protected override void Awake()
    {        
        base.Awake();

        currentProjectileSpeed = weaponData.ProjectileSpeed;
        currentPierce = weaponData.Pierce;       
    }

    protected override void Start()
    {
        base .Start();

        if (applyRotation && direction != Vector3.zero)
        {
            RotateTowardsDirection();
        }
    }

    public void DirectionChecker(Vector2 dir)
    {
        direction = dir.normalized;
        if (applyRotation) RotateTowardsDirection();
    }

    private void RotateTowardsDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += angleOffset; // Применяем сдвиг
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
    }

    protected override void AfterDamageApplied()
    {
        ReducePierce();
    }

    void ReducePierce()
    {
        currentPierce--;

        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
