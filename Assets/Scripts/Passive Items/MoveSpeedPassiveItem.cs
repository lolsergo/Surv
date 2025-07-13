using UnityEngine;

public class MoveSpeed : PassiveItem
{
    protected override void ApplyModifier()
    {
        currentPlayerStats.currentMoveSpeed *= 1 + passiveItemData.Multipler / 100f;
    }
}
