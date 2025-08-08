using UnityEngine;

public abstract class InventoryItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private string _itemName;
    public string ItemName { get => _itemName; private set => _itemName = value; }

    [SerializeField]
    private int _upgradableItemLevel;
    public int UpgradableItemLevel { get => _upgradableItemLevel; private set => _upgradableItemLevel = value; }

    [SerializeField]
    private GameObject _nextLevelPrefab;
    public GameObject NextLevelPrefab { get => _nextLevelPrefab; private set => _nextLevelPrefab = value; }

    [SerializeField]
    private string _itemDescription;
    public string ItemDescription { get => _itemDescription; private set => _itemDescription = value; }

    [SerializeField]
    Sprite _icon;
    public Sprite Icon { get => _icon; private set => _icon = value; }

}
