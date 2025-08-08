using TMPro;
using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        UpdateGoldDisplay();
    }

    public void UpdateGoldDisplay()
    {
        // Загружаем данные текущего профиля
        ProfileData profile = SaveSystem.LoadCurrentProfile();

        // Обновляем текст
        goldText.text = $"Gold: {profile.totalGold}";
    }
}
