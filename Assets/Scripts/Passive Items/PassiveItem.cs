using UnityEngine;

public class PassiveItem : MonoBehaviour,IInventoryItem
{
    protected PlayerController currentPlayerStats;
    public PassiveItemScriptableObject passiveItemData;
    public int ItemLevel { get => passiveItemData.UpgradableItemLevel; }
    public GameObject NextLevelPrefab { get => passiveItemData.NextLevelPrefab; }
    public string ItemName  { get => passiveItemData.PassiveItemName; }

    protected virtual void ApplyModifier()
    {

    }

    void Start()
    {
        currentPlayerStats = FindFirstObjectByType<PlayerController>();
        ApplyModifier();
    }
}
