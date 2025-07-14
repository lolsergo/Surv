using UnityEngine;

public class PassiveItem : MonoBehaviour,IInventoryItem
{
    protected CurrentPlayerStats currentPlayerStats;
    public PassiveItemScriptableObject passiveItemData;
    public int ItemLevel { get => passiveItemData.UpgradableItemLevel; }
    public GameObject NextLevelPrefab { get => passiveItemData.NextLevelPrefab; }

    protected virtual void ApplyModifier()
    {

    }

    void Start()
    {
        currentPlayerStats = FindFirstObjectByType<CurrentPlayerStats>();
        ApplyModifier();
    }
}
