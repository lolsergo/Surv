using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public CurrentPlayerStats currentPlayerStats;

    float currentMoveSpeed;
    float currentHealth;
    float currentDamage;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentPlayerStats.IncreaseExpirience(enemyData.ExpPerKill);
            Kill();
        }
    }

    public void Kill() 
    {
        Destroy(gameObject);
    }
}
