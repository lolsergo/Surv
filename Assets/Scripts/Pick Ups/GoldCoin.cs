using UnityEngine;

public class GoldCoin : MonoBehaviour, iCollectible
{
    int goldGranted;
    public int maxGoldGranted;

    void Awake()
    {
        goldGranted = Random.Range(1, maxGoldGranted + 1);
    }

    public void Collect()
    {
        CurrentPlayerStats player = FindFirstObjectByType<CurrentPlayerStats>();
        player.IncreaseGold(goldGranted);
        Destroy(gameObject);
    }
}
