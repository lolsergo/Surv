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

        // ���������� AvailableProfiles �� ProfileManager ������ ������� ������� � ������
        foreach (string profileName in ProfileManager.AvailableProfiles)
        {
            CreateProfileButton(profileName);
        }
    }

    private void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
        deleteModeButton.GetComponentInChildren<TMP_Text>().text =
            isDeleteMode ? "��������" : "������� �������";

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
        ProfileManager.CreateProfile(name, error => {
            // ����� ����� �������� ��������� �� ������ ������������
            Debug.LogError(error);
        });

        RefreshProfiles();
        newProfileInput.text = "";
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
            // ������ ��������� �������� �������
            ProfileManager.SwitchProfile(profileName);
            SceneManager.LoadScene("Title Screen");
        }
    }

    public void ConfirmDelete()
    {
        if (!string.IsNullOrEmpty(selectedProfileToDelete))
        {
            ProfileManager.DeleteProfile(selectedProfileToDelete);
            RefreshProfiles();
        }
        deleteConfirmPanel.SetActive(false);
    }

    public void CancelDelete()
    {
        deleteConfirmPanel.SetActive(false);
    }
}
