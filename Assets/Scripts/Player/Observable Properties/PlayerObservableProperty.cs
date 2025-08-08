using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PlayerObservableProperty<T>
{
    public event Action<T> OnValueChanged;

    private T _value;
    private readonly StatType _statType; // Тип свойства для идентификации

    public PlayerObservableProperty(StatType statType)
    {
        _statType = statType;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
                UpdateUI(); // Автоматическое обновление UI
            }
        }
    }

    private void UpdateUI()
    {
        if (GameManager.instance == null) return;

        TMP_Text targetText = _statType switch
        {
            StatType.Health => GameManager.instance.currentHealthDisplay,
            StatType.HealthRegen => GameManager.instance.currentRegenDisplay,
            StatType.MoveSpeed => GameManager.instance.currentMoveSpeedDisplay,
            StatType.Might => GameManager.instance.currentMightDisplay,
            StatType.Gold => GameManager.instance.currentGoldDisplay,
            StatType.Magnet => GameManager.instance.currentMagnetDisplay,
            _ => null
        };

        if (targetText != null)
            targetText.text = FormatValue(_value); // Форматируем значение
    }

    public void ForceUpdateUI()
    {
        UpdateUI(); // Принудительно обновляем текст
    }

    private string FormatValue(T value)
    {
        return _statType switch
        {
            StatType.Health => $"HP: {value:F0}",
            StatType.HealthRegen => $"Regen: {value:F0}/s",
            StatType.MoveSpeed => $"Move Speed: {value:F0}",
            StatType.Might => $"Might: {value:F0}",
            StatType.Gold => $"Gold: {value:F0}",
            StatType.Magnet => $"Magnet: {value:F1}",
            _ => value.ToString()
        };
    }
}

public enum StatType
{
    Health,
    HealthRegen,
    MoveSpeed,
    Might,
    Gold,
    Magnet
}
