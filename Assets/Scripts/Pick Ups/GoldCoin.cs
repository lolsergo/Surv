using UnityEngine;

public class GoldCoin : PickUps
{
    private int goldGranted;
    public int maxGoldGranted;

    private void Awake()
    {
        goldGranted = Random.Range(1, maxGoldGranted + 1);
    }

    protected override void OnCollected()
    {
        base.OnCollected();
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.CurrentGold.IncreaseGold(goldGranted);
    }
}
