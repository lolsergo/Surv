using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CurrentPlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentHealthRegen;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentMagnetRadius;

    public List<GameObject> spawnedWeapons;

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
    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

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
        GameObject spawnedWeapon= Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon);
    }
}
