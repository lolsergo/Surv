using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    CharacterScriptableObject characterData;

    public PlayerHealth CurrentHealth = new();
    public PlayerHealthRegen CurrentHealthRegen = new();
    public MoveSpeed CurrentMoveSpeed = new();
    public PlayerMight CurrentMight = new();
    public PlayerMagnet CurrentMagnetRadius = new();
    public PlayerGold CurrentGold = new();
    [SerializeField]
    private int baseGold;

    [Header("Experience/Level")]
    private int experience;
    private int level = 1;
    private int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startingLevel;
        public int endingLevel;
        public int experienceCapIncrease;
    }

    [Header("I-Frames")]
    public float invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;

    public List<LevelRange> levelRanges;

    [SerializeField]
    private InventoryManager inventory;

    [SerializeField]
    private UpgradeLevelsConfig upgradeLevelsConfig;
    private int possibleMaxLevel;
    private WeaponController baseWeapon;
    private int baseWeaponLevel;
    private PassiveItem basePassiveItem;
    private int basePassiveItemLevel;
    private int possibleWeaponLevels;
    private int possiblePassiveItemLevels;

    [Header("UI")]
    public Image healthBar;
    public Image expirienceBar;
    public TMP_Text levelText;

    private float _lastDisplayedHealth;

    [SerializeField]
    private ParticleSystem damageEffect;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        InitializeBaseStats(characterData);
        InitializeStartingEquipment(characterData);
        CalculateLevelCaps();
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        CurrentHealth.OnDamaged += damage => StartInvincibility();
        CurrentHealth.OnDeath += Kill;

        GameManager.instance.AssignChosenCharacterUI(characterData);

        UpdateExpirienceBar();
        UpdateLevelText();
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }

        CurrentHealth.Regeneration(CurrentHealthRegen.Value);

        UpdateHealthBar();
    }

    public void IncreaseExpirience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            StartCoroutine(ProcessLevelUps());
        }
        UpdateExpirienceBar();
    }

    private IEnumerator ProcessLevelUps()
    {
        while (experience >= experienceCap && level < possibleMaxLevel)
        {
            level++;
            AnalyticsManager.Instance.LevelUp(level);
            UpdateLevelText();
            int previousCap = experienceCap;

            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startingLevel && level <= range.endingLevel)
                {
                    experienceCap += range.experienceCapIncrease;
                    break;
                }
            }

            experience -= previousCap;
            UpdateExpirienceBar();
            GameManager.instance.StartLevelUp();
            yield return new WaitUntil(() => !GameManager.instance.isChoosingUpgrades);
            if (experience < experienceCap) break;

        }
    }

    public void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            CurrentHealth.TryTakeDamage(damage);
            if (damageEffect)
            {
                Instantiate(damageEffect, transform.position, Quaternion.identity);
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (CurrentHealth.Value < characterData.MaxHealth &&
        Mathf.Abs(CurrentHealth.Value - _lastDisplayedHealth) > 0.1f) // Фиксированный порог
        {
            healthBar.fillAmount = CurrentHealth.Value / characterData.MaxHealth;
            _lastDisplayedHealth = CurrentHealth.Value;
        }
    }

    private void UpdateExpirienceBar()
    {
        expirienceBar.fillAmount = (float)experience / experienceCap;
    }

    private void UpdateLevelText()
    {
        levelText.text = $"level {level}";
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignSpriteUI(GameManager.instance.chosenWeaponsUI, inventory.weaponSlots);
            GameManager.instance.AssignSpriteUI(GameManager.instance.chosenPassiveItemsUI, inventory.passiveSlots);
            GameManager.instance.GameOver();

            Destroy(gameObject, 0.1f);
        }
    }

    public void SpawnWeapon(GameObject weaponPrefab)
    {
        if (weaponPrefab == null) return;
        SpawnInventoryItem(weaponPrefab);
    }

    public void ApplyPassive(GameObject passiveItemPrefab)
    {
        if (passiveItemPrefab == null) return;
        SpawnInventoryItem(passiveItemPrefab);
    }

    public void SpawnInventoryItem(GameObject itemPrefab)
    {
        GameObject appliedItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        appliedItem.transform.SetParent(transform);
        if (appliedItem.TryGetComponent<IInventoryItem>(out var upgradableItem))
        {
            inventory.AddItem(upgradableItem);
        }
        else
        {
            Debug.LogError($"Объект {itemPrefab.name} не имеет компонента IInventoryItem");
        }
    }

    private void InitializeBaseStats(CharacterScriptableObject data)
    {
        CurrentHealth.Initialize(data.MaxHealth);
        CurrentHealthRegen.Value = data.HealthRegen;
        CurrentMight.Value = data.Might;
        CurrentMoveSpeed.Value = data.MoveSpeed;
        CurrentMagnetRadius.Value = data.MagnetRadius;
        CurrentGold.Value = baseGold;
        CurrentGold.ForceUpdateUI();
    }

    private void InitializeStartingEquipment(CharacterScriptableObject data)
    {
        baseWeapon = data.BaseWeapon.GetComponent<WeaponController>()
    ?? throw new MissingComponentException($"No WeaponController on {data.BaseWeapon.name}");
        baseWeaponLevel = baseWeapon.ItemLevel;

        basePassiveItem = data.BasePassiveItem.GetComponent<PassiveItem>()
        ?? throw new MissingComponentException($"No PassiveItem on {data.BasePassiveItem.name}");
        basePassiveItemLevel = basePassiveItem.ItemLevel;

        SpawnWeapon(data.BaseWeapon);
        ApplyPassive(data.BasePassiveItem);
    }

    private void CalculateLevelCaps()
    {
        possibleWeaponLevels = upgradeLevelsConfig.weapons.Sum(entry => entry.maxLevel);
        possiblePassiveItemLevels = upgradeLevelsConfig.passiveItems.Sum(entry => entry.maxLevel);
        possibleMaxLevel = possibleWeaponLevels + possiblePassiveItemLevels
                         - baseWeaponLevel - basePassiveItemLevel;
    }
}
