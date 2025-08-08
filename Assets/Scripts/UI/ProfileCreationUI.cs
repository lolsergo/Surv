using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCreationUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Button _createButton;
    [SerializeField] private TMP_Text _errorText;

    [Header("Settings")]
    [SerializeField] private Color _errorColor = Color.red;
    [SerializeField] private float _errorShowTime = 3f;

    private void Awake()
    {
        _createButton.onClick.AddListener(CreateProfile);
        _nameInput.onValueChanged.AddListener(ValidateInput);
        _errorText.gameObject.SetActive(false);
    }

    private void ValidateInput(string text)
    {
        _createButton.interactable = text.Length >= 3 && text.Length <= 16;
    }

    public void CreateProfile()
    {
        string profileName = _nameInput.text.Trim();

        if (ProfileManager.CreateProfile(profileName))
        {
            // Успешное создание
            gameObject.SetActive(false);
        }
        else
        {
            ShowError("Не удалось создать профиль");
        }
    }

    private void ShowError(string message)
    {
        _errorText.text = message;
        _errorText.color = _errorColor;
        _errorText.gameObject.SetActive(true);
        Invoke(nameof(HideError), _errorShowTime);
    }

    private void HideError()
    {
        _errorText.gameObject.SetActive(false);
    }
}
