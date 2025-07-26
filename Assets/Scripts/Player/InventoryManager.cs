using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

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
    }

    [SerializeField]
    public SerializedDictionary<WeaponController, int> weapons;
    [SerializeField]
    public SerializedDictionary<Image, string> weaponSlots = new();

    [SerializeField]
    public SerializedDictionary<PassiveItem, int> passives;
    [SerializeField]
    public SerializedDictionary<Image, string> passiveSlots = new();

    public void AddItem(IInventoryItem item)
    {
        switch (item)
        {
            case WeaponController weapon:
                weapons.Add(weapon, (weapon.weaponData.UpgradableItemLevel));
                AddImage(weapon.weaponData.ItemName, weapon.weaponData.Icon, weaponSlots);
                break;
            case PassiveItem passive:
                passives.Add(passive, (passive.passiveItemData.UpgradableItemLevel));
                AddImage(passive.passiveItemData.ItemName, passive.passiveItemData.Icon, passiveSlots);
                break;
            default:
                throw new ArgumentException("Неизвестный тип предмета");
        }

        if (GameManager.instance != null && GameManager.instance.isChoosingUpgrades)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddImage(string itemName, Sprite itemSprite, SerializedDictionary<Image, string> dictionary)
    {
        if (dictionary.ContainsValue(itemName))
        {
            return;
        }

        foreach (var slot in dictionary.Keys.ToList())
        {
            if (string.IsNullOrEmpty(dictionary[slot]))
            {
                dictionary[slot] = itemName;
                slot.sprite = itemSprite;
                slot.enabled = true;
                return;
            }
        }
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
}
