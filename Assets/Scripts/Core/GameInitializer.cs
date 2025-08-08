using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO; // Для Directory
using System; // Для DateTime

public class GameInitializer : MonoBehaviour
{
    // Настройки в инспекторе
    [SerializeField]
    private string _titleScreenScene = "Title Screen";
    [SerializeField]
    private bool _resetProfilesOnStart = false; // Для дебага

    private void Awake()
    {
        InitializeProfiles();
        LoadMainMenu();
        DontDestroyOnLoad(gameObject); // Чтобы не уничтожался при загрузке сцен
    }

    private void InitializeProfiles()
    {
        // Дебаг-очистка (опционально)
        if (_resetProfilesOnStart && Directory.Exists(ProfileManager.ProfilesDirectory))
        {
            Directory.Delete(ProfileManager.ProfilesDirectory, true);
            Debug.Log("Профили сброшены!");
        }

        // Инициализация профилей
        if (ProfileManager.AvailableProfiles.Count == 0)
        {
            string defaultName = "Player_" + DateTime.Now.ToString("yyyyMMdd");
            ProfileManager.CreateProfile(defaultName);
            ProfileManager.SwitchProfile(defaultName);
            Debug.Log($"Создан новый профиль: {defaultName}");
        }
        else if (string.IsNullOrEmpty(ProfileManager.CurrentProfile))
        {
            ProfileManager.SwitchProfile(ProfileManager.AvailableProfiles[0]);
        }
    }

    private void LoadMainMenu()
    {
        if (!string.IsNullOrEmpty(_titleScreenScene))
        {
            SceneManager.LoadScene(_titleScreenScene);
        }
        else
        {
            Debug.LogError("Название сцены главного меню не задано!");
        }
    }
}