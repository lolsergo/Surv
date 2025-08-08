using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour, IInventoryItem
{
    [SerializeReference]
    public WeaponScriptableObject weaponData;
    public int ItemLevel { get => weaponData.UpgradableItemLevel; }
    public GameObject NextLevelPrefab { get => weaponData.NextLevelPrefab; }
    public string ItemName { get => weaponData.ItemName; }
    public string ItemDescription {  get => weaponData.ItemDescription; }
    public Sprite Icon { get => weaponData.Icon; }

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

    public void UpgradeInInventory(InventoryManager inventory)
    {
        inventory.LevelUpItem(this,
                            inventory.weapons,
                            w => w.weaponData.NextLevelPrefab);
    }

    public bool CanUpgrade() =>
       ItemLevel < GameManager.instance.upgradeConfig.GetWeaponMaxLevel(weaponData.ItemName);
}
