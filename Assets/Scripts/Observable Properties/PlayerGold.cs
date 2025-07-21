using UnityEngine;

public class PlayerGold : PlayerObservableProperty<int>
{
    public PlayerGold() : base(StatType.Gold) { }

    public void IncreaseGold(int amount)
    {
        Value += amount;
    }
}
