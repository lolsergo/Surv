using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CurrentPlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    [ReadOnly]
    public float currentHealth;
    [ReadOnly]
    public float currentHealthRegen;
    [ReadOnly]
    public float currentMoveSpeed;
    [ReadOnly]
    public float currentMight;
    [ReadOnly]
    public float currentMagnetRadius;

    [Header("Experience/Level")]
    public int experience;
    public int level;

    public int currentGold;

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
    float invincibilityTimer;
    bool isInvincibile;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentHealthRegen = characterData.HealthRegen;
        currentMight = characterData.Might;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMagnetRadius = characterData.MagnetRadius;

        SpawnWeapon(characterData.BaseWeapon);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    private void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincibile)
        {
            isInvincibile = false;
        }

        Recover();
    }

    public void IncreaseExpirience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    public void IncreaseGold(int amount)
    {
        currentGold += amount;
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > characterData.MaxHealth)
        {
            currentHealth = characterData.MaxHealth;
        }
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

    public void TakeDamage(float damage)
    {
        if (!isInvincibile)
        {
            currentHealth -= damage;

            invincibilityTimer = invincibilityDuration;
            isInvincibile = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("YOU DIED");
    }

    public bool IsHealthFull()
    {
        return currentHealth == characterData.MaxHealth;
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentHealthRegen * Time.deltaTime;

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
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
