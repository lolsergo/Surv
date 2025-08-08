using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // Пути (вариант B)
    private static string SavesDir => ProfileManager.ProfilesDirectory;
    private static string CurrentProfilePath => Path.Combine(SavesDir, $"{ProfileManager.CurrentProfile}.save");
    private static string LegacySavePath => Path.Combine(Application.persistentDataPath, "meta_save.json");

    // Основные методы
    public static void SaveCurrentProfile(ProfileData data)
    {
        if (string.IsNullOrEmpty(ProfileManager.CurrentProfile))
        {
            Debug.LogError("No active profile selected!");
            return;
        }

        data.lastPlayed = DateTime.Now;
        SaveProfile(ProfileManager.CurrentProfile, data);
    }

    public static void SaveProfile(string profileName, ProfileData data)
    {
        EnsureDirectoryExists();
        string path = Path.Combine(SavesDir, $"{profileName}.save");
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }

    public static ProfileData LoadCurrentProfile()
    {
        if (string.IsNullOrEmpty(ProfileManager.CurrentProfile))
        {
            Debug.LogWarning("Loading default profile data");
            return new ProfileData("Default");
        }

        return LoadProfile(ProfileManager.CurrentProfile);
    }

    public static ProfileData LoadProfile(string profileName)
    {
        string path = Path.Combine(SavesDir, $"{profileName}.save");

        if (!File.Exists(path))
        {
            // Попытка миграции старых данных
            if (profileName == "Default" && File.Exists(LegacySavePath))
            {
                return MigrateLegacyData();
            }
            return new ProfileData(profileName);
        }

        try
        {
            return JsonUtility.FromJson<ProfileData>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load profile: {e}");
            return new ProfileData(profileName);
        }
    }

    // Миграция данных
    private static ProfileData MigrateLegacyData()
    {
        try
        {
            var legacyData = JsonUtility.FromJson<MetaProgressData>(File.ReadAllText(LegacySavePath));
            var profile = new ProfileData("Migrated")
            {
                totalGold = legacyData.TotalGold,
                boughtUpgrades = legacyData.BoughtUpgrades ?? new List<string>()
            };
            SaveProfile("Migrated", profile);
            File.Delete(LegacySavePath);
            return profile;
        }
        catch
        {
            return new ProfileData("Default");
        }
    }

    private static void EnsureDirectoryExists()
    {
        if (!Directory.Exists(SavesDir))
            Directory.CreateDirectory(SavesDir);
    }
}