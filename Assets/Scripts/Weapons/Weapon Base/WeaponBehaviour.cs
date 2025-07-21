using Unity.VisualScripting;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [HideInInspector]
    public WeaponScriptableObject weaponData;

    protected float BaseDamage => weaponData.Damage;
    protected float CurrentDamage { get; private set; }

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
        PlayerController currentPlayerStats = FindFirstObjectByType<PlayerController>();
        CurrentDamage = weaponData.Damage * currentPlayerStats.CurrentMight.Value;
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
        target.TakeDamage(CurrentDamage);
        AfterDamageApplied();
    }

    protected virtual void AfterDamageApplied() { }
}
