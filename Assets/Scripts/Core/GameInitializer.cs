using System;
using System.IO;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameInitializer : MonoBehaviour
{
    [Header("���������")]
    [SerializeField]
    private string _titleScreenScene = "Title Screen";
    [SerializeField]
    private bool _resetProfilesOnStart = false;
    [SerializeField]
    private bool _enableUnityServices = true;

    private async void Awake() // ��������� Awake, �� ������ �����������
    {
        try
        {
            // 1. ������������� UGS (���� ��������)
            if (_enableUnityServices)
            {
                await InitializeUnityServices();
            }

            // 2. ������� ������������� ����
            InitializeProfiles();
            LoadMainMenu();

            DontDestroyOnLoad(gameObject);
        }
        catch (Exception e)
        {
            Debug.LogError($"������������� �� �������: {e.Message}");
            // ������: ��������� ����� ��� ��������
            SceneManager.LoadScene(_titleScreenScene);
        }
    }

    private async Task InitializeUnityServices()
    {
        if (UnityServices.State != ServicesInitializationState.Uninitialized)
        {
            Debug.LogWarning("Unity Services ��� ����������������");
            return;
        }

        await UnityServices.InitializeAsync();
        Debug.Log("Unity Services ������ � ������");

        // ����� ����� �������� ������������� ���������� �������� �����
        // ��������: Analytics, Cloud Save � �.�.
    }

    private void InitializeProfiles()
    {
        // �����-�������
        if (_resetProfilesOnStart && Directory.Exists(ProfileManager.ProfilesDirectory))
        {
            Directory.Delete(ProfileManager.ProfilesDirectory, true);
            Debug.Log("[DEBUG] ������� ��������");
        }

        // ������������� ��������
        if (ProfileManager.AvailableProfiles.Count == 0)
        {
            string defaultName = "Player_" + DateTime.Now.ToString("yyyyMMdd");
            ProfileManager.CreateProfile(defaultName);
            ProfileManager.SwitchProfile(defaultName);
            Debug.Log($"������ ������� �� ���������: {defaultName}");
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
            Debug.LogError("�������� ����� �� ������ � ����������!");
            return;
        }

        SceneManager.LoadScene(_titleScreenScene);
    }
}