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

    public void IncreaseExpirience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    public void IncreaseGold(int amount)
    {
        currentGold += amount;
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
}
