using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<int> availableWeaponIndices = new(weaponUpgradeOptions.Count);
        for (int i = 0; i < weaponUpgradeOptions.Count; i++)
        {
            availableWeaponIndices.Add(i);
        }

        Debug.Log($"Сейчас доступно {availableWeaponIndices.Count} оружий");

        List<int> availablePassiveIndices = new(passiveItemUpgradeOptions.Count);
        for (int i = 0; i < passiveItemUpgradeOptions.Count; i++)
        {
            availablePassiveIndices.Add(i);
        }


        foreach (var upgradeOption in upgradeUIOptions)
        {
            bool canChooseWeapon = availableWeaponIndices.Count > 0;
            bool canChoosePassive = availablePassiveIndices.Count > 0;

            if (!canChooseWeapon && !canChoosePassive)
            {
                Debug.Log("Нет доступных апгрейдов для кнопки!");
                DisableUpgradeUI(upgradeOption);
                continue;
            }

            int upgradeType;
            if (canChooseWeapon && canChoosePassive)
            {
                upgradeType = UnityEngine.Random.Range(0, 2);
            }
            else
            {
                upgradeType = canChooseWeapon ? 0 : 1;
            }

            if (upgradeType == 0)
            {
                ChooseUpgradeWeapon(upgradeOption, availableWeaponIndices);
            }
            else
            {
                ChooseUpgradePassiveItem(upgradeOption, availablePassiveIndices);
            }
        }
    }

    private void ChooseUpgradeWeapon(UpgradeUI upgradeOption, List<int> availableWeaponIndices)
    {
        int randomListIndex = UnityEngine.Random.Range(0, availableWeaponIndices.Count);
        int originalIndex = availableWeaponIndices[randomListIndex];
        RemoveUsedIndex(availableWeaponIndices, randomListIndex);

        WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[originalIndex];

        bool isWeaponOwned = InventoryManager.weaponSlots.ContainsValue(chosenWeaponUpgrade.weaponData.ItemName);

        EnableUpgradeUI(upgradeOption);

        if (isWeaponOwned)
        {
            ChooseUpgradeOnClick(chosenWeaponUpgrade.weaponData, upgradeOption, originalIndex);
        }
        else
        {
            AddNewItemOnClick(chosenWeaponUpgrade.initialWeapon, upgradeOption);
        }

        AddUpgradeIcon(chosenWeaponUpgrade.weaponData, upgradeOption);
        AddDescription(isWeaponOwned, upgradeOption, chosenWeaponUpgrade.weaponData);
    }

    private void ChooseUpgradePassiveItem(UpgradeUI upgradeOption, List<int> availablePassiveIndices)
    {
        int randomListIndex = UnityEngine.Random.Range(0, availablePassiveIndices.Count);
        int originalIndex = availablePassiveIndices[randomListIndex];
        RemoveUsedIndex(availablePassiveIndices, randomListIndex);

        PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[originalIndex];

        bool isPassiveOwned = InventoryManager.passiveSlots.ContainsValue(chosenPassiveItemUpgrade.passiveItemData.ItemName);

        EnableUpgradeUI(upgradeOption);

        if (isPassiveOwned)
        {
            ChooseUpgradeOnClick(chosenPassiveItemUpgrade.passiveItemData, upgradeOption, originalIndex);
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
    private void ChooseUpgradeOnClick(IInventoryItem item, UpgradeUI upgradeOption, int itemIndex)
    {
        upgradeOption.upgradeButton.onClick.AddListener(() =>
        {
            item.UpgradeInInventory(InventoryManager);
            UpdateItemData(itemIndex, item);
            RemoveMaxLevelItems();
        });
    }

    private void UpdateItemData(int itemIndex, IInventoryItem newItem)
    {
        if (newItem is WeaponController weapon)
        {
            weaponUpgradeOptions[itemIndex].initialWeapon = newItem.NextLevelPrefab;
            if (newItem.NextLevelPrefab.TryGetComponent(out WeaponController newWeapon))
            {
                weaponUpgradeOptions[itemIndex].weaponData = newWeapon;
                Debug.Log($"Updated weapon: {newWeapon.ItemName} (Lvl {newWeapon.ItemLevel})");
            }
            else
            {
                Debug.LogError("No PassiveItem component on NextLevelPrefab!");
            }
        }
        else if (newItem is PassiveItem passiveItem)
        {
            passiveItemUpgradeOptions[itemIndex].initialPassiveItem = newItem.NextLevelPrefab;
            if (newItem.NextLevelPrefab.TryGetComponent(out PassiveItem newPassiveItem))
            {
                passiveItemUpgradeOptions[itemIndex].passiveItemData = newPassiveItem;
                Debug.Log($"Updated passive item: {newPassiveItem.ItemName} (Lvl {newPassiveItem.ItemLevel})");
            }
            else
            {
                Debug.LogError("No PassiveItem component on NextLevelPrefab!");
            }
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
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        if (weaponUpgradeOptions.Count > 0 || passiveItemUpgradeOptions.Count > 0)
        {
            RemoveUpgradeOptions();
            ApplyUpgradeOptions();
        }
    }

    public void RemoveMaxLevelItems()
    {
        RemoveMaxLevelWeapons();
        RemoveMaxLevelPassiveItems();
    }

    public void RemoveMaxLevelWeapons()
    {
        weaponUpgradeOptions.RemoveAll(upgrade =>
            upgrade.weaponData != null && !upgrade.weaponData.CanUpgrade()
        );
    }

    public void RemoveMaxLevelPassiveItems()
    {
        passiveItemUpgradeOptions.RemoveAll(upgrade =>
            upgrade.passiveItemData != null && !upgrade.passiveItemData.CanUpgrade()
        );
    }

    public void RemoveUsedIndex(List<int> list, int index)
    {
        list.RemoveAt(index);
    }

    private void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDispay.transform.parent.gameObject.SetActive(false);
    }

    private void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDispay.transform.parent.gameObject.SetActive(true);
    }
}
