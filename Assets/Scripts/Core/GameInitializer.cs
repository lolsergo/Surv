using System;
using System.IO;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameInitializer : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField]
    private string _titleScreenScene = "Title Screen";
    [SerializeField]
    private bool _resetProfilesOnStart = false;
    [SerializeField]
    private bool _enableUnityServices = true;

    private async void Awake() // Оставляем Awake, но делаем асинхронным
    {
        try
        {
            // 1. Инициализация UGS (если включено)
            if (_enableUnityServices)
            {
                await InitializeUnityServices();
            }

            // 2. Базовая инициализация игры
            InitializeProfiles();
            LoadMainMenu();

            DontDestroyOnLoad(gameObject);
        }
        catch (Exception e)
        {
            Debug.LogError($"Инициализация не удалась: {e.Message}");
            // Фолбэк: загружаем сцену без сервисов
            SceneManager.LoadScene(_titleScreenScene);
        }
    }

    private async Task InitializeUnityServices()
    {
        if (UnityServices.State != ServicesInitializationState.Uninitialized)
        {
            Debug.LogWarning("Unity Services уже инициализированы");
            return;
        }

        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services готовы к работе");

        // Здесь можно добавить инициализацию конкретных сервисов позже
        // Например: Analytics, Cloud Save и т.д.
    }

    private void InitializeProfiles()
    {
        // Дебаг-очистка
        if (_resetProfilesOnStart && Directory.Exists(ProfileManager.ProfilesDirectory))
        {
            Directory.Delete(ProfileManager.ProfilesDirectory, true);
            Debug.Log("[DEBUG] Профили сброшены");
        }

        // Инициализация профилей
        if (ProfileManager.AvailableProfiles.Count == 0)
        {
            string defaultName = "Player_" + DateTime.Now.ToString("yyyyMMdd");
            ProfileManager.CreateProfile(defaultName);
            ProfileManager.SwitchProfile(defaultName);
            Debug.Log($"Создан профиль по умолчанию: {defaultName}");
        }
        else if (string.IsNullOrEmpty(ProfileManager.CurrentProfile))
        {
            ProfileManager.SwitchProfile(ProfileManager.AvailableProfiles[0]);
        }
    }

    private void LoadMainMenu()
    {
        if (string.IsNullOrEmpty(_titleScreenScene))
        {
            Debug.LogError("Название сцены не задано в инспекторе!");
            return;
        }

        SceneManager.LoadScene(_titleScreenScene);
    }
}