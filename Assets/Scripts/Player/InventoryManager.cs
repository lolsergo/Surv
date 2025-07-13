using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private SerializedDictionary<WeaponController, int> weapons;

    [SerializeField]
    private SerializedDictionary<PassiveItem, int> passives;

    public void AddWeapon(WeaponController weapon, int level)
    {
        weapons.Add(weapon, level);
    }

    public void AddPassive(PassiveItem passive, int level)
    {
        passives.Add(passive, level);
    }

    public void LevelUpWeapon(WeaponController weapon)
    {

    }

    public void LevelUpPassive(PassiveItem passiveItem)
    {

    }
}
