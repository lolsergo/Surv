using UnityEngine;
using static EnemySpawner;

public class EnemyStats : MonoBehaviour, IDamageable
{
    public EnemyScriptableObject enemyData;
    private DropRateManager _dropRateManager;

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

        _dropRateManager = GetComponent<DropRateManager>();
        if (_dropRateManager == null) Debug.LogError("DropRateManager не найден!", this);
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
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
            EnemyDie();
        }
    }

    private void EnemyDie()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.IncreaseExpirience(enemyData.ExpPerKill);
        _dropRateManager.Die();
        EnemySpawner enemySpawner = FindFirstObjectByType<EnemySpawner>();
        enemySpawner.OnEnemyKilled();

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.ApplyDamage(currentDamage);
        }
    }

    void ReturnEnemy()
    {      
        Vector2 spawnOffset = Random.insideUnitCircle.normalized * enemyData.SpawnRadius;
        transform.position = (Vector2)player.position + spawnOffset;
    }
}
