using UnityEngine;

public class HealthPotion : PickUps
{
    public int healthGranted;

    protected override void OnCollected()
    {
        base.OnCollected();
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.CurrentHealth.Heal(healthGranted);
    }
}
