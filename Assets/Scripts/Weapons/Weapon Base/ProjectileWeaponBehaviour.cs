using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableIObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSecons;
    public bool applyRotation = true;
    public float angleOffset = 0f;

    protected float currentDamage;
    protected float currentProjectileSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentProjectileSpeed = weaponData.ProjectileSpeed;
        currentPierce = weaponData.Pierce;
        currentCooldownDuration = weaponData.CooldownDuration;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecons);

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

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.TryGetComponent(out BreakableProps breakableProps))
            {
                breakableProps.TakeDamage(currentDamage);
                ReducePierce();
            }
        }
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
