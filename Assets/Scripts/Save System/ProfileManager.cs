using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class ProfileManager
{
    public static string ProfilesDirectory => Path.Combine(Application.persistentDataPath, "Saves");
    private static string GetProfilePath(string name) => Path.Combine(ProfilesDirectory, $"{name}.save");

    // Текущее состояние
    public static List<string> AvailableProfiles { get; private set; } = new List<string>();
    public static string CurrentProfile { get; private set; }

    public static event Action OnProfileCreated;

    // Инициализация
    static ProfileManager()
    {
        InitializeProfiles();
    }

    private static void InitializeProfiles()
    {
        if (!Directory.Exists(ProfilesDirectory))
            Directory.CreateDirectory(ProfilesDirectory);

        RefreshProfilesList();

        if (AvailableProfiles.Count == 0)
            CreateProfile("Default");
    }

    // Основные методы
    public static bool CreateProfile(string name, Action<string> onError = null)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            onError?.Invoke("Name cannot be empty");
            Debug.LogError("[Profile] Creation failed: empty name");
            return false;
        }

        if (name.Length < 3 || name.Length > 16)
        {
            onError?.Invoke("Name must be 3-16 characters");
            Debug.LogError("[Profile] Creation failed: invalid length");
            return false;
        }

        if (name.Any(ch => !char.IsLetterOrDigit(ch) && ch != '_'))
        {
            onError?.Invoke("Only letters, numbers and _ allowed");
            Debug.LogError("[Profile] Creation failed: invalid characters");
            return false;
        }

        string path = GetProfilePath(name);
        if (File.Exists(path))
        {
            onError?.Invoke("Profile already exists");
            Debug.LogError($"[Profile] Creation failed: {name} already exists");
            return false;
        }

        try
        {
            ProfileData newProfile = new(name);
            SaveSystem.SaveProfile(name, newProfile);
            RefreshProfilesList();

            if (string.IsNullOrEmpty(CurrentProfile))
                SwitchProfile(name);

            Debug.Log($"[Profile] Created: {name}");
            OnProfileCreated?.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Profile] Creation error: {ex.Message}");
            onError?.Invoke("System error");
            return false;
        }
    }

    public static bool DeleteProfile(string name)
    {
        string path = GetProfilePath(name);
        if (!File.Exists(path)) return false;

        File.Delete(path);
        if (CurrentProfile == name)
            CurrentProfile = null;

        RefreshProfilesList();
        return true;
    }

    public static bool SwitchProfile(string name)
    {
        if (!AvailableProfiles.Contains(name)) return false;

        CurrentProfile = name;
        return true;
    }

    // Вспомогательные методы
    private static void RefreshProfilesList()
    {
        AvailableProfiles = Directory.GetFiles(ProfilesDirectory, "*.save")
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();
    }
}