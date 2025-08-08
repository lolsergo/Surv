using UnityEngine;

public class PassiveItem : MonoBehaviour,IInventoryItem
{
    protected PlayerController currentPlayerStats;
    public PassiveItemScriptableObject passiveItemData;
    public int ItemLevel { get => passiveItemData.UpgradableItemLevel; }
    public GameObject NextLevelPrefab { get => passiveItemData.NextLevelPrefab; }
    public string ItemName  { get => passiveItemData.ItemName; }
    public string ItemDescription { get => passiveItemData.ItemDescription; }
    public Sprite Icon { get => passiveItemData.Icon; }

    protected virtual void ApplyModifier()
    {

    }

    void Start()
    {
        currentPlayerStats = FindFirstObjectByType<PlayerController>();
        ApplyModifier();
    }

    public void UpgradeInInventory(InventoryManager inventory)
    {
        inventory.LevelUpItem(this,
                            inventory.passives,
                            w => w.passiveItemData.NextLevelPrefab);
    }

    public bool CanUpgrade() =>
        ItemLevel < GameManager.instance.upgradeConfig.GetPassiveItemMaxLevel(passiveItemData.ItemName);
}
