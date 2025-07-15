using UnityEngine;

public interface IInventoryItem
{
    GameObject NextLevelPrefab { get; }
    int ItemLevel { get; }
    string ItemName { get; }
}
