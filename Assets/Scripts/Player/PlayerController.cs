using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int experience;
    private int level;
    [HideInInspector]
    public int experienceCap;

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

    InventoryManager inventory;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        CurrentHealth.Initialize(characterData.MaxHealth);
        CurrentHealthRegen.Value = characterData.HealthRegen;
        CurrentMight.Value = characterData.Might;
        CurrentMoveSpeed.Value = characterData.MoveSpeed;
        CurrentMagnetRadius.Value = characterData.MagnetRadius;
        CurrentGold.Value = baseGold;
        CurrentGold.ForceUpdateUI();

        SpawnWeapon(characterData.BaseWeapon);
        ApplyPassive(characterData.BasePassiveItem);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        CurrentHealth.OnDamaged += damage => StartInvincibility();
        CurrentHealth.OnDeath += Kill;

        GameManager.instance.AssignChosenCharacterUI(characterData);
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
    }

    public void IncreaseExpirience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        // Запускаем корутину, если есть опыт для хотя бы одного уровня
        if (experience >= experienceCap)
        {
            StartCoroutine(ProcessLevelUps());
        }
    }

    private IEnumerator ProcessLevelUps()
    {
        while (experience >= experienceCap)
        {
            // 1. Повышаем уровень
            level++;

            // 2. Сохраняем старый cap перед изменением
            int previousCap = experienceCap;

            // 3. Находим текущий диапазон и увеличиваем cap
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startingLevel && level <= range.endingLevel)
                {
                    experienceCap += range.experienceCapIncrease; // Ваш оригинальный подход!
                    break;
                }
            }

            // 4. Вычитаем опыт (только за текущий уровень)
            experience -= previousCap;

            // 5. Активируем экран улучшений
            GameManager.instance.StartLevelUp();

            // 6. Ждем выбора улучшения
            yield return new WaitUntil(() => !GameManager.instance.isChoosingUpgrades);

            // 7. Проверяем, хватает ли опыта на следующий уровень
            if (experience < experienceCap) break;
        }
    }

    public void ApplyDamage(float damage)
    {
        if (!isInvincible)
        {
            CurrentHealth.TryTakeDamage(damage);
        }
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
}
