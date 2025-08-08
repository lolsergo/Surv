using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text profileNameText;
    private string profileName;
    public string ProfileName => profileName;
    private ProfileSelectionUI controller;
    [SerializeField]
    private GameObject deleteIcon;

    public void Setup(string name, ProfileSelectionUI uiController)
    {
        profileName = name;
        controller = uiController;
        profileNameText.text = name;
        deleteIcon.SetActive(false);

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        controller.OnProfileSelected(profileName);
    }

    public void ShowDeleteIcon(bool show)
    {
        deleteIcon.SetActive(show);
    }
}
