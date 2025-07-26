using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UpgradeSystem : MonoBehaviour
{
    private InventoryManager InventoryManager => InventoryManager.Instance;

    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon;
        public WeaponController weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public GameObject initialPassiveItem;
        public PassiveItem passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDispay;
        public TMP_Text upgradeDescriptionDispay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new();
    public List<UpgradeUI> upgradeUIOptions = new();

    PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void ApplyUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            int upgradeType = UnityEngine.Random.Range(0, 2);
            if (upgradeType == 0)
            {
                ChooseUpgradeWeapon(upgradeOption);
            }
            else
            {
                ChooseUpgradePassiveItem(upgradeOption);
            }
        }
    }

    private void ChooseUpgradeWeapon(UpgradeUI upgradeOption)
    {
        int weaponIndex = UnityEngine.Random.Range(0, weaponUpgradeOptions.Count);

        WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[weaponIndex];

        bool isWeaponOwned = InventoryManager.weaponSlots.ContainsValue(chosenWeaponUpgrade.weaponData.ItemName);

        if (isWeaponOwned)
        {
            ChooseUpgradeOnClick(chosenWeaponUpgrade.weaponData, upgradeOption, weaponUpgradeOptions[weaponIndex].initialWeapon, weaponIndex);
        }
        else
        {
            AddNewItemOnClick(chosenWeaponUpgrade.initialWeapon, upgradeOption);
        }

        AddUpgradeIcon(chosenWeaponUpgrade.weaponData, upgradeOption);
        AddDescription(isWeaponOwned, upgradeOption, chosenWeaponUpgrade.weaponData);
    }

    private void ChooseUpgradePassiveItem(UpgradeUI upgradeOption)
    {
        int passiveIndex = UnityEngine.Random.Range(0, passiveItemUpgradeOptions.Count);

        PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[passiveIndex];

        bool isPassiveOwned = InventoryManager.passiveSlots.ContainsValue(chosenPassiveItemUpgrade.passiveItemData.ItemName);

        if (isPassiveOwned)
        {
            ChooseUpgradeOnClick(chosenPassiveItemUpgrade.passiveItemData, upgradeOption, passiveItemUpgradeOptions[passiveIndex].initialPassiveItem, passiveIndex);
        }
        else
        {
            AddNewItemOnClick(chosenPassiveItemUpgrade.initialPassiveItem, upgradeOption);
        }

        AddUpgradeIcon(chosenPassiveItemUpgrade.passiveItemData, upgradeOption);
        AddDescription(isPassiveOwned, upgradeOption, chosenPassiveItemUpgrade.passiveItemData);
    }

    private void AddDescription(bool isItemOwned, UpgradeUI upgradeOption, IInventoryItem item)
    {
        if (isItemOwned)
        {
            IInventoryItem nextLevelInventoryItem = item.NextLevelPrefab.GetComponent<IInventoryItem>();
            upgradeOption.upgradeDescriptionDispay.text = nextLevelInventoryItem.ItemDescription;
            upgradeOption.upgradeNameDispay.text = nextLevelInventoryItem.ItemName + $" level {nextLevelInventoryItem.ItemLevel}";
        }
        else
        {
            upgradeOption.upgradeDescriptionDispay.text = item.ItemDescription;
            upgradeOption.upgradeNameDispay.text = item.ItemName + $" level {item.ItemLevel}";
        }
    }
    private void ChooseUpgradeOnClick(IInventoryItem item, UpgradeUI upgradeOption, GameObject initialObject, int itemIndex)
    {
        upgradeOption.upgradeButton.onClick.AddListener(() =>
        {
            item.UpgradeInInventory(InventoryManager);

            UpdateItemData(itemIndex, item);
        });
    }

    private void UpdateItemData(int itemIndex, IInventoryItem newItem)
    {
        if (newItem is WeaponController weapon)
        {
            weaponUpgradeOptions[itemIndex].initialWeapon = newItem.NextLevelPrefab;
            weaponUpgradeOptions[itemIndex].weaponData = weapon;
        }
        else if (newItem is PassiveItem passiveItem)
        {
            passiveItemUpgradeOptions[itemIndex].initialPassiveItem = newItem.NextLevelPrefab;
            passiveItemUpgradeOptions[itemIndex].passiveItemData = passiveItem;
        }
    }

    private void AddNewItemOnClick(GameObject item, UpgradeUI upgradeOption)
    {
        upgradeOption.upgradeButton.onClick.AddListener(() =>
        {
            player.SpawnInventoryItem(item);
        });
    }

    private void AddUpgradeIcon(IInventoryItem item, UpgradeUI upgradeOption)
    {
        upgradeOption.upgradeIcon.sprite = item.Icon;
    }

    private void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
}
