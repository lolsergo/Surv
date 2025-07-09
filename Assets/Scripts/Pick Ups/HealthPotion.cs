using UnityEngine;

public class HealthPotion : MonoBehaviour, iCollectible
{
    public int healthGranted;

    public void Collect()
    {
        CurrentPlayerStats player = FindFirstObjectByType<CurrentPlayerStats>();
        if (!player.IsHealthFull())
        {
            player.RestoreHealth(healthGranted);
            Destroy(gameObject);
        }
    }
}
