using UnityEngine;

public interface IInventoryItem
{
    GameObject NextLevelPrefab { get; }
    int ItemLevel { get; }
    string ItemName { get; }
    Sprite Icon { get; }
    string ItemDescription {  get; }
    void UpgradeInInventory(InventoryManager inventory);
}
