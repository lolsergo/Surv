using UnityEngine;

public class PlayerHealthRegen : PlayerObservableProperty<float>
{
    public PlayerHealthRegen() : base(StatType.HealthRegen) { }
}
