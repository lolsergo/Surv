using UnityEngine;
using static EnemySpawner;

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
    Transform player;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;

        dropRateManager = GetComponent<DropRateManager>();
        if (dropRateManager == null) Debug.LogError("DropRateManager не найден!", this);
    }

    void Start()
    {
        player = FindFirstObjectByType<CurrentPlayerStats>().transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) >= enemyData.DespawnDistance)
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            CurrentPlayerStats player = FindFirstObjectByType<CurrentPlayerStats>();
            player.IncreaseExpirience(enemyData.ExpPerKill);
            dropRateManager.Die();
            EnemySpawner enemySpawner = FindFirstObjectByType<EnemySpawner>();
            enemySpawner.OnEnemyKilled();
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

    void ReturnEnemy()
    {
        EnemySpawner enemySpawner = FindFirstObjectByType<EnemySpawner>();
        Vector2 spawnOffset = Random.insideUnitCircle.normalized * enemyData.SpawnRadius;
        transform.position = (Vector2)player.position + spawnOffset;
    }
}
