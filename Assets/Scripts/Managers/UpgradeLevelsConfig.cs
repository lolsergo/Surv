using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeLevelsConfig", menuName = "Configs/Upgrade Levels")]
public class UpgradeLevelsConfig : ScriptableObject
{
    [System.Serializable]
    public class WeaponEntry
    {
        public WeaponScriptableObject weaponSO;  // Перетаскиваем SO сюда
        public int maxLevel;          // Ручное назначение уровня

        public string ItemId => weaponSO != null ? weaponSO.ItemName : "INVALID";

        [ReadOnly]
        public string itemNameDisplay; // Только для показа в инспекторе

        // Обновляем поле при изменении SO
        public void OnValidate()
        {
            itemNameDisplay = weaponSO != null ? weaponSO.ItemName : "NULL (Drag SO here)";
        }
    }

    [System.Serializable]
    public class PassiveItemEntry
    {
        public PassiveItemScriptableObject itemSO;  // Перетаскиваем SO сюда
        public int maxLevel;          // Ручное назначение уровня

        public string ItemId => itemSO != null ? itemSO.ItemName : "INVALID";

        [ReadOnly]
        public string itemNameDisplay;

        public void OnValidate()
        {
            itemNameDisplay = itemSO != null ? itemSO.ItemName : "NULL (Drag SO here)";
        }
    }

    public List<WeaponEntry> weapons;
    public List<PassiveItemEntry> passiveItems;

    private Dictionary<string, int> _weaponLevels;
    private Dictionary<string, int> _passiveItemLevels;

    public void Initialize()
    {
        _weaponLevels = new Dictionary<string, int>();
        foreach (var entry in weapons)
        {
            if (entry.weaponSO != null)
                _weaponLevels[entry.ItemId] = entry.maxLevel;
        }

        _passiveItemLevels = new Dictionary<string, int>();
        foreach (var entry in passiveItems)
        {
            if (entry.itemSO != null)
                _passiveItemLevels[entry.ItemId] = entry.maxLevel;
        }
    }

    public int GetWeaponMaxLevel(string itemId) =>
        _weaponLevels.TryGetValue(itemId, out int level) ? level : 0;

    public int GetPassiveItemMaxLevel(string itemId) =>
        _passiveItemLevels.TryGetValue(itemId, out int level) ? level : 0;

    private void OnValidate()
    {
        // Обновляем display-поля при изменении SO в инспекторе
        foreach (var entry in weapons) entry.OnValidate();
        foreach (var entry in passiveItems) entry.OnValidate();
    }
}