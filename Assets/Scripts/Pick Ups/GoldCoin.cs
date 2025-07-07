using UnityEngine;

public class GoldCoin : MonoBehaviour, iCollectible
{
    public int goldGranted;

    public void Collect()
    {
        CurrentPlayerStats player = FindFirstObjectByType<CurrentPlayerStats>(); 
        player.IncreaseGold(goldGranted);
        Destroy(gameObject);
    }
}
