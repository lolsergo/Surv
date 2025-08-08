using System;
using UnityEngine;

public class PlayerGold : PlayerObservableProperty<int>
{
    public PlayerGold() : base(StatType.Gold) { }

    public void Add(int amount) => Value += amount;

    // ��������� ������ � ����-�������� (���������� ��� ���������� ������)
    public void SaveToProfile()
    {
        // ��������� ������ �������� �������
        ProfileData profile = SaveSystem.LoadCurrentProfile();

        // ��������� ��������
        profile.totalGold += this.Value;
        profile.lastPlayed = DateTime.Now;

        // ��������� ���������
        SaveSystem.SaveCurrentProfile(profile);
    }
}
