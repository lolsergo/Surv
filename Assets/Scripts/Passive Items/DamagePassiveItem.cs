using UnityEngine;

public class DamagePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        currentPlayerStats.currentMight *= 1 + passiveItemData.Multipler / 100f;
    }
}
