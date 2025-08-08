using System;
using UnityEngine;

public class PlayerGold : PlayerObservableProperty<int>
{
    public PlayerGold() : base(StatType.Gold) { }

    public void Add(int amount) => Value += amount;

    // Сохраняет золото в мета-прогресс (вызывается при завершении забега)
    public void SaveToProfile()
    {
        // Загружаем данные текущего профиля
        ProfileData profile = SaveSystem.LoadCurrentProfile();

        // Обновляем значения
        profile.totalGold += this.Value;
        profile.lastPlayed = DateTime.Now;

        // Сохраняем изменения
        SaveSystem.SaveCurrentProfile(profile);
    }
}
