using UnityEngine;

public interface IInventoryItem
{
    GameObject NextLevelPrefab { get; }
    int ItemLevel { get; }
}
