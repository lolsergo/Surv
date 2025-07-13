using Unity.VisualScripting;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableIObject weaponData;

    [SerializeField]
    protected float currentDamage;
    [SerializeField]
    protected float currentCooldownDuration;
    [SerializeField]
    protected float destroyAfterSecons;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecons);
    }

    protected virtual void Awake()
    {
        CurrentPlayerStats currentPlayerStats = FindFirstObjectByType<CurrentPlayerStats>();
        currentDamage = weaponData.Damage * currentPlayerStats.currentMight;
        currentCooldownDuration = weaponData.CooldownDuration;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            ApplyDamage(collider.GetComponent<EnemyStats>());
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.TryGetComponent(out BreakableProps prop))
            {
                ApplyDamage(prop);
            }
        }
    }

    protected virtual void ApplyDamage(IDamageable target)
    {
        target.TakeDamage(currentDamage);
        AfterDamageApplied();
    }

    protected virtual void AfterDamageApplied() { }
}
