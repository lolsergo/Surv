using UnityEngine;

public class BreakableProps : MonoBehaviour, IDamageable
{
    private DropRateManager dropRateManager;

    public float health;

    private void Awake()
    {
        dropRateManager = GetComponent<DropRateManager>();
        if (dropRateManager == null) Debug.LogError("ItemDropper не найден!", this);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            dropRateManager.Die();
            Destroy(gameObject);
        }
    }
}
