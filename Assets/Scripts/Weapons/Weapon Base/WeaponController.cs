using UnityEngine;

public class WeaponController : MonoBehaviour, IInventoryItem
{
    [HideInInspector]
    public WeaponScriptableObject weaponData;
    public int ItemLevel { get => weaponData.UpgradableItemLevel; }
    public GameObject NextLevelPrefab { get => weaponData.NextLevelPrefab; }
    float currentCooldown;


    protected PlayerMovement playerMovement;

    protected virtual void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration;
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
