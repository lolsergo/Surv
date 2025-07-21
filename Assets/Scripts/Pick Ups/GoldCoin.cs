using UnityEngine;

public class GoldCoin : PickUps, ICollectible
{
    int goldGranted;
    public int maxGoldGranted;

    void Awake()
    {
        goldGranted = Random.Range(1, maxGoldGranted + 1);
    }

    public void Collect()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.CurrentGold.IncreaseGold(goldGranted);
    }
}
