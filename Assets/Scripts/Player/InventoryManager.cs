using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UpgradeSystem;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private UpgradeSystem _upgradeSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Если нужно сохранять между сценами
        }
        else
        {
            Destroy(gameObject);
        }

        _upgradeSystem = GetComponent<UpgradeSystem>();
    }

    [SerializeField]
    public SerializedDictionary<WeaponController, int> weapons;
    [SerializeField]
    public SerializedDictionary<Image, string> weaponSlots = new();

    [SerializeField]
    public SerializedDictionary<PassiveItem, int> passives;
    [SerializeField]
    public SerializedDictionary<Image, string> passiveSlots = new();

    public List<WeaponEvolutionBlueprint> weaponEvolutions = new();

    public void AddItem(IInventoryItem item)
    {
        switch (item)
        {
            case WeaponController weapon:
                weapons.Add(weapon, (weapon.weaponData.UpgradableItemLevel));
                AddOrUpdateImage(weapon.weaponData.ItemName, weapon.weaponData.Icon, weaponSlots);
                break;
            case PassiveItem passive:
                passives.Add(passive, (passive.passiveItemData.UpgradableItemLevel));
                AddOrUpdateImage(passive.passiveItemData.ItemName, passive.passiveItemData.Icon, passiveSlots);
                break;
            default:
                throw new ArgumentException("Неизвестный тип предмета");
        }

        if (GameManager.instance != null && GameManager.instance.isChoosingUpgrades)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddOrUpdateImage(string itemName, Sprite newSprite, SerializedDictionary<Image, string> dictionary)
    {
        // 1. Сначала проверяем, есть ли уже такой предмет
        foreach (var pair in dictionary)
        {
            if (pair.Value == itemName)
            {
                // Обновляем спрайт для существующего
                pair.Key.sprite = newSprite;
                pair.Key.enabled = true;
                return;
            }
        }

        // 2. Если предмет новый - ищем пустой слот
        foreach (var slot in dictionary.Keys.ToList())
        {
            if (string.IsNullOrEmpty(dictionary[slot]))
            {
                dictionary[slot] = itemName;
                slot.sprite = newSprite;
                slot.enabled = true;
                return;
            }
        }

        // 3. Если нет свободных слотов
        Debug.LogWarning($"No empty slots available for {itemName}");
    }

    public void LevelUpItem<T>(T item, SerializedDictionary<T, int> dictionary, Func<T, GameObject> getNextLevelPrefab)
        where T : MonoBehaviour, IInventoryItem
    {
        var key = dictionary.Keys.FirstOrDefault(k => k.ItemName == item.ItemName);

        if (key == null)
        {
            Debug.Log($"wrong key {key}");
        }

        if (dictionary.ContainsKey(key))
        {
            if (key == null)
            {
                Debug.LogError("Item is null!");
                return;
            }

            GameObject nextPrefab = getNextLevelPrefab(item);
            if (nextPrefab == null)
            {
                Debug.LogError("Next level prefab is null!");
                return;
            }

            // Создаем улучшенную версию
            GameObject upgradedItem = Instantiate(
                nextPrefab,
                transform.position,
                Quaternion.identity
            );
            upgradedItem.transform.SetParent(transform);

            T newItem = upgradedItem.GetComponent<T>();

            // Удаляем старое, добавляем новое
            dictionary.Remove(key);
            Destroy(key.gameObject);

            AddItem(newItem);
            upgradedItem.transform.SetParent(transform);
        }
        else
        {
            Debug.LogError($"Item {key.name} not found in dictionary!");
            return;
        }

        if (GameManager.instance != null && GameManager.instance.isChoosingUpgrades)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(WeaponController weapon)
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon is null!");
            return;
        }

        Debug.Log($"Trying to upgrade weapon: {weapon.name}");
        LevelUpItem(weapon, weapons, w => w.weaponData.NextLevelPrefab);
    }

    public void LevelUpPassive(PassiveItem passive)
    {
        if (passive == null)
        {
            Debug.LogError("Passive is null!");
            return;
        }

        LevelUpItem(passive, passives, p => p.passiveItemData.NextLevelPrefab);
    }

    public List<WeaponEvolutionBlueprint> GetPossibleEvolutions()
    {
        return weaponEvolutions
            .Where(evo =>
                // Проверяем оружие (единственное в инвентаре)
                weapons.Keys.Any(w =>
                    w.weaponData.ItemName == evo.baseWeaponData.ItemName &&
                    w.weaponData.UpgradableItemLevel >= evo.baseWeaponData.UpgradableItemLevel) &&

                // Проверяем пассивку (единственную в инвентаре)
                passives.Keys.Any(p =>
                    p.passiveItemData.ItemName == evo.catalystPassiveItemData.ItemName &&
                    p.passiveItemData.UpgradableItemLevel >= evo.catalystPassiveItemData.UpgradableItemLevel)
            )
            .ToList();
    }

    public bool TryExecuteEvolution(WeaponEvolutionBlueprint evolution)
    {
        // 1. Ищем нужное оружие среди ключей словаря
        WeaponController playerWeapon = weapons.Keys
            .FirstOrDefault(w => w.weaponData.ItemName == evolution.baseWeaponData.ItemName);

        if (playerWeapon == null)
        {
            Debug.LogWarning($"Не найдено базовое оружие: {evolution.baseWeaponData.ItemName}");
            return false;
        }

        // 2. Ищем нужную пассивку (аналогично)
        PassiveItem playerPassive = passives.Keys
            .FirstOrDefault(p => p.passiveItemData.ItemName == evolution.catalystPassiveItemData.ItemName);

        if (playerPassive == null)
        {
            Debug.LogWarning($"Не найдена пассивка: {evolution.catalystPassiveItemData.ItemName}");
            return false;
        }

        // 3. Проверяем уровни
        if (playerWeapon.weaponData.UpgradableItemLevel < evolution.baseWeaponData.UpgradableItemLevel ||
            playerPassive.passiveItemData.UpgradableItemLevel < evolution.catalystPassiveItemData.UpgradableItemLevel)
        {
            Debug.Log("Недостаточный уровень оружия/пассивки");
            return false;
        }

        // 4. Если всё ок — запускаем эволюцию
        ExecuteEvolution(evolution, playerWeapon);
        return true;
    }

    private void ExecuteEvolution(WeaponEvolutionBlueprint evolution, WeaponController weapon)
    {
        GameObject evolvedWeapon = Instantiate(evolution.evolvedWeapon, transform.position, Quaternion.identity);
        WeaponController evolvedWeaponController = evolvedWeapon.GetComponent<WeaponController>();

        evolvedWeapon.transform.SetParent(transform);
        weapons.Remove(weapon);
        ReplaceEvolvedWeaponData(evolvedWeaponController);
        AddItem(evolvedWeaponController);
        Destroy(weapon.gameObject);
    }

    private void ReplaceEvolvedWeaponData(WeaponController evolvedWeapon)
    {
        int index = _upgradeSystem.weaponUpgradeOptions.FindIndex(upgrade => upgrade.weaponData.ItemName == evolvedWeapon.ItemName);

        if (index != -1)
        {
            _upgradeSystem.weaponUpgradeOptions[index] = new WeaponUpgrade
            {
                initialWeapon = evolvedWeapon.weaponData.Prefab,
                weaponData = evolvedWeapon
            };
        }

        _upgradeSystem.RemoveMaxLevelWeapons();
    }
}
