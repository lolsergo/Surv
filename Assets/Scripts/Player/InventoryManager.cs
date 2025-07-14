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
                throw new ArgumentException("����������� ��� ��������");
        }
    }

    public void LevelUpItem<T>(T item, SerializedDictionary<T, int> dictionary, Func<T, GameObject> getNextLevelPrefab)
        where T : MonoBehaviour, IInventoryItem
    {
        if (dictionary.ContainsKey(item))
        {
            // ������� ���������� ������
            GameObject upgradedItem = Instantiate(
                getNextLevelPrefab(item),
                item.transform.position,
                item.transform.rotation
            );

            T newItem = upgradedItem.GetComponent<T>();

            // ������� ������, ��������� �����
            dictionary.Remove(item);
            Destroy(item.gameObject);

            dictionary.Add(newItem, newItem.ItemLevel);
            upgradedItem.transform.SetParent(transform);
        }
    }
}
