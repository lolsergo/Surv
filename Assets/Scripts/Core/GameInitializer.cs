using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO; // ��� Directory
using System; // ��� DateTime

public class GameInitializer : MonoBehaviour
{
    // ��������� � ����������
    [SerializeField]
    private string _titleScreenScene = "Title Screen";
    [SerializeField]
    private bool _resetProfilesOnStart = false; // ��� ������

    private void Awake()
    {
        InitializeProfiles();
        LoadMainMenu();
        DontDestroyOnLoad(gameObject); // ����� �� ����������� ��� �������� ����
    }

    private void InitializeProfiles()
    {
        // �����-������� (�����������)
        if (_resetProfilesOnStart && Directory.Exists(ProfileManager.ProfilesDirectory))
        {
            Directory.Delete(ProfileManager.ProfilesDirectory, true);
            Debug.Log("������� ��������!");
        }

        // ������������� ��������
        if (ProfileManager.AvailableProfiles.Count == 0)
        {
            string defaultName = "Player_" + DateTime.Now.ToString("yyyyMMdd");
            ProfileManager.CreateProfile(defaultName);
            ProfileManager.SwitchProfile(defaultName);
            Debug.Log($"������ ����� �������: {defaultName}");
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
            Debug.LogError("�������� ����� �������� ���� �� ������!");
        }
    }
}