using UnityEngine;

public class MagnetPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        currentPlayerStats.currentMagnetRadius *= 1 + passiveItemData.Multipler / 100f;
    }
}
