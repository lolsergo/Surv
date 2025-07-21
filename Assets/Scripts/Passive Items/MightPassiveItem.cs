using UnityEngine;

public class MightPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        currentPlayerStats.CurrentMight.Value *= 1 + passiveItemData.Multipler / 100f;
    }
}
