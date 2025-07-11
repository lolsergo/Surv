using UnityEngine;

public class HealthPotion : PickUps, ICollectible
{
    public int healthGranted;

    public void Collect()
    {
        CurrentPlayerStats player = FindFirstObjectByType<CurrentPlayerStats>();
        if (!player.IsHealthFull())
        {
            player.RestoreHealth(healthGranted);
        }
    }
}
