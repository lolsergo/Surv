using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public SerializedDictionary<WeaponController, int> weapons;

    [SerializeField]
    private SerializedDictionary<PassiveItem, int> passives;

    public void AddItem(IInventoryItem item)
    {
        switch (item)
        {
            case WeaponController weapon:
                weapons.Add(weapon, weapon.ItemLevel);
                break;
            case PassiveItem passive:
                passives.Add(passive, passive.ItemLevel);
                break;
            default:
                throw new ArgumentException("Неизвестный тип предмета");
        }
    }

    public void LevelUpItem<T>(T item, SerializedDictionary<T, int> dictionary, Func<T, GameObject> getNextLevelPrefab)
        where T : MonoBehaviour, IInventoryItem
    {
        if (dictionary.ContainsKey(item))
        {
            // Создаем улучшенную версию
            GameObject upgradedItem = Instantiate(
                getNextLevelPrefab(item),
                item.transform.position,
                item.transform.rotation
            );

            T newItem = upgradedItem.GetComponent<T>();

            // Удаляем старое, добавляем новое
            dictionary.Remove(item);
            Destroy(item.gameObject);

            dictionary.Add(newItem, newItem.ItemLevel);
            upgradedItem.transform.SetParent(transform);
        }
    }
}
