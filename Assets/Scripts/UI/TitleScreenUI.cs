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
        // ��������� ������ �������� �������
        ProfileData profile = SaveSystem.LoadCurrentProfile();

        // ��������� �����
        goldText.text = $"Gold: {profile.totalGold}";
    }
}
