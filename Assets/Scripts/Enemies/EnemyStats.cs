using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    private DropRateManager dropRateManager;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;

        dropRateManager=GetComponent<DropRateManager>();
        if (dropRateManager == null) Debug.LogError("ItemDropper не найден!", this);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            CurrentPlayerStats player = FindFirstObjectByType<CurrentPlayerStats>();
            player.IncreaseExpirience(enemyData.ExpPerKill);
            dropRateManager.Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CurrentPlayerStats playerStats = collision.gameObject.GetComponent<CurrentPlayerStats>();
            playerStats.TakeDamage(currentDamage);
        }
    }
}
