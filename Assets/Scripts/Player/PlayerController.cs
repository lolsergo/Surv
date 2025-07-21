using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

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
        if (experience >= experienceCap)
        {
            foreach (LevelRange levelRange in levelRanges)
            {
                while (level <= levelRange.endingLevel && experience > experienceCap)
                {
                    level++;
                    experience -= experienceCap;
                }

                experienceCap += levelRange.experienceCapIncrease;
            }

            if (experience >= experienceCap)
            {
                level += (experience / experienceCap);
                experience -= ((experience / experienceCap) * experienceCap);
            }
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
            GameManager.instance.GameOver();
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        IInventoryItem upgradableItem = spawnedWeapon.GetComponent<IInventoryItem>();
        inventory.AddItem(upgradableItem);
    }

    public void ApplyPassive(GameObject passiveItem)
    {
        GameObject appliedPassive = Instantiate(passiveItem, transform.position, Quaternion.identity);
        appliedPassive.transform.SetParent(transform);
        IInventoryItem upgradableItem = appliedPassive.GetComponent<IInventoryItem>();
        inventory.AddItem(upgradableItem);
    }
}
