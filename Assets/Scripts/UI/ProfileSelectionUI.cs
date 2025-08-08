using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileSelectionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform profilesContainer;
    [SerializeField]
    private GameObject profileButtonPrefab;
    [SerializeField]
    private TMP_InputField newProfileInput;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button deleteModeButton;
    [SerializeField]
    private GameObject deleteConfirmPanel;

    private bool isDeleteMode = false;
    private string selectedProfileToDelete;

    private void Start()
    {
        createButton.onClick.AddListener(CreateProfile);
        deleteModeButton.onClick.AddListener(ToggleDeleteMode);
        RefreshProfiles();
        deleteConfirmPanel.SetActive(false);
    }

    private void RefreshProfiles()
    {
        // ������� ������ ������
        foreach (Transform child in profilesContainer)
            Destroy(child.gameObject);

        // ������� ������ ��� ������� �������
        string[] profiles = Directory.GetFiles(Application.persistentDataPath, "*.profile");
        foreach (string path in profiles)
        {
            string profileName = Path.GetFileNameWithoutExtension(path);
            CreateProfileButton(profileName);
        }
    }

    private void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
        deleteModeButton.GetComponentInChildren<TMP_Text>().text =
            isDeleteMode ? "��������" : "������� �������";

        // ��������� ��� ������
        foreach (Transform child in profilesContainer)
        {
            var button = child.GetComponent<ProfileButton>();
            if (button) button.ShowDeleteIcon(isDeleteMode);
        }
    }

    private void CreateProfileButton(string name)
    {
        GameObject button = Instantiate(profileButtonPrefab, profilesContainer);
        button.GetComponent<ProfileButton>().Setup(name, this);
    }

    public void SelectProfile(string name)
    {
        // ����� ����� ������ ������ �������
        Debug.Log($"Selected profile: {name}");
    }

    public void CreateProfile()
    {
        string name = newProfileInput.text.Trim();
        if (!string.IsNullOrEmpty(name))
        {
            // ������� ���� �������
            File.WriteAllText(
                Path.Combine(Application.persistentDataPath, $"{name}.profile"),
                "{}"
            );
            RefreshProfiles();
            newProfileInput.text = "";
        }
    }

    public void OnProfileSelected(string profileName)
    {
        if (isDeleteMode)
        {
            selectedProfileToDelete = profileName;
            deleteConfirmPanel.SetActive(true);
        }
        else
        {
            // ������� ����� �������
            ProfileManager.SwitchProfile(profileName);
            SceneManager.LoadScene("Title Screen");
        }
    }

    public void ConfirmDelete()
    {
        string path = Path.Combine(Application.persistentDataPath, $"{selectedProfileToDelete}.profile");
        if (File.Exists(path))
        {
            File.Delete(path);

            // ������� � ������� ������ ��������������� ������
            foreach (Transform child in profilesContainer)
            {
                var button = child.GetComponent<ProfileButton>();
                if (button != null && button.ProfileName == selectedProfileToDelete)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
        deleteConfirmPanel.SetActive(false);
    }

    public void CancelDelete()
    {
        deleteConfirmPanel.SetActive(false);
    }
}
