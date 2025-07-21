using UnityEngine;

public class MagnetPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        currentPlayerStats.CurrentMagnetRadius.Value *= 1 + passiveItemData.Multipler / 100f;
    }
}
