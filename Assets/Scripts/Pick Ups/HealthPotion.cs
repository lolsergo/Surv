using UnityEngine;

public class HealthPotion : PickUps, ICollectible
{
    public int healthGranted;

    public void Collect()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.CurrentHealth.Heal(healthGranted);
    }
}
