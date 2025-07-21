using System;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

[System.Serializable]
public class PlayerHealth : PlayerObservableProperty<float>
{
    public PlayerHealth() : base(StatType.Health) { }

    private float _maxHealth;

    public void Initialize(float maxHealth)
    {
        _maxHealth = maxHealth;
        Value = _maxHealth; // ������������� ��������� ��������
    }

    public void Heal(int amount)
    {
        if (Value < _maxHealth)
        {
            Value += amount;

            if (Value > _maxHealth)
            {
                Value = _maxHealth;
            }
        }
    }

    public void Regeneration(float healthRegen)
    {
        if (Value < _maxHealth)
        {
            Value += healthRegen * Time.deltaTime;

            if (Value > _maxHealth)
            {
                Value = _maxHealth;
            }
        }
    }

    public bool IsHealthFull()
    {
        return Value == _maxHealth;
    }

    public event Action<float> OnDamaged; // float - ���������� ����
    public event Action OnDeath;

    public bool TryTakeDamage(float damage)
    {
        if (Value <= 0) return false; // ��� �����

        Value -= damage;
        OnDamaged?.Invoke(damage); // ���������� � ���������� �����

        if (Value <= 0)
        {
            OnDeath?.Invoke();
            return true; // ���������� true, ���� ������
        }

        return false;
    }
}