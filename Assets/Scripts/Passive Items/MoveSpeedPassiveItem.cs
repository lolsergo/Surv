using UnityEngine;

public class MoveSpeedPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        currentPlayerStats.CurrentMoveSpeed.Value *= 1 + passiveItemData.Multipler / 100f;
    }
}
