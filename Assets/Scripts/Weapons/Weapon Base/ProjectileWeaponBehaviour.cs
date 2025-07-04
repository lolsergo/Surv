using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableIObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSecons;
    public bool applyRotation = true;
    public float angleOffset = 0f;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.FlightSpeed;
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

    //public void DirectionChecker(Vector2 dir)
    //{
    //    direction = dir.normalized;

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //    transform.rotation = Quaternion.Euler(0, 0, angle);

    //    //float dirX = direction.x;
    //    //float dirY = direction.y;

    //    //Vector3 scale = transform.localScale;
    //    //Vector3 rotation = transform.rotation.eulerAngles;

    //    //if (dirX < 0 && dirY == 0)
    //    //{
    //    //    scale.x *= -1;
    //    //    scale.y *= -1;
    //    //}

    //    //transform.localScale = scale;
    //    //transform.rotation = Quaternion.Euler(rotation);
    //}

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
