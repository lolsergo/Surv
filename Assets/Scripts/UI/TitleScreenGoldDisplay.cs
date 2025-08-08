public class TitleScreenGoldDisplay : GoldDisplay
{
    private void Start()
    {
        LoadAndDisplayGold();
    }

    public void LoadAndDisplayGold()
    {
        ProfileData currentProfile = SaveSystem.LoadCurrentProfile();
        // Обновляем отображение
        UpdateDisplay(currentProfile.totalGold);
    }
}