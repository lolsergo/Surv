using UnityEngine;

public class PlayerMagnet : PlayerObservableProperty<float>
{
    public PlayerMagnet() : base(StatType.Magnet) { }
}
