using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public WeaponScriptableObject expectedWeaponData;

    [SerializeField]
    public SerializedDictionary<WeaponController, int> weapons;

    [SerializeField]
    private SerializedDictionary<PassiveItem, int> passives;

    public void AddItem(IInventoryItem item)
    {
        switch (item)
        {
            case WeaponController weapon:
                weapons.Add(weapon, weapon.weaponData.UpgradableItemLevel);
                break;
            case PassiveItem passive:
                passives.Add(passive, passive.passiveItemData.UpgradableItemLevel);
                break;
            default:
                throw new ArgumentException("Неизвестный тип предмета");
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
        LevelUpItem(passive, passives, p => p.passiveItemData.NextLevelPrefab);
    }
}
