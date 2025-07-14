using UnityEngine;

public interface IUpgradableItem 
{
    GameObject NextLevelPrefab { get; }
    int UpgradableItemLevel { get; }
}