using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableIObject weaponData;

    public float destroyAfterSecons;

    protected float currentDamage;
    protected float currentCooldownDuration;

    private void Awake()
    {
        currentDamage = weaponData.Damage;
        currentCooldownDuration = weaponData.CooldownDuration;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecons);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.TryGetComponent(out BreakableProps breakableProps))
            {
                breakableProps.TakeDamage(currentDamage);
            }
        }
    }
}
