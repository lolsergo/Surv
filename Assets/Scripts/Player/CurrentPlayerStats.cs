using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CurrentPlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    float currentHealth;
    float currentHealthRegen;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    public int currentGold = 0;

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
    void Awake()
    {
        currentHealth = characterData.MaxHealth;
        currentHealthRegen = characterData.HealthRegen;
        currentMight = characterData.Might;
        currentMoveSpeed = characterData.MoveSpeed;
        currentProjectileSpeed = characterData.ProjectileSpeed;
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
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;

            foreach (LevelRange levelRange in levelRanges)
            {
                if (level >= levelRange.startingLevel && level <= levelRange.endingLevel)
                {
                    experienceCapIncrease = levelRange.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
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
}
