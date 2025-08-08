using System.Collections;
using UnityEngine;
using static EnemySpawner;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour, IDamageable, IKnockbackable
{
    public EnemyScriptableObject enemyData;
    private DropRateManager _dropRateManager;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    private float knockbackResistance;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new();
    public float damageFlashDuration;
    public float deathFadeTime;
    private Color originalColor;
    private SpriteRenderer sr;
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
        knockbackResistance = enemyData.KnockbackResistance;

        _dropRateManager = GetComponent<DropRateManager>();
        if (_dropRateManager == null) Debug.LogError("DropRateManager не найден!", this);
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        enemyMovement = GetComponent<EnemyMovement>();
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
        StartCoroutine(DamageFlash());

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

        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        WaitForEndOfFrame w = new();
        float t = 0, origAlpha = sr.color.a;

        while (t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
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

    public void ApplyKnockback(Vector2 sourcePosition, float knockbackForce, float KnockbackDuration)
    {
        if (knockbackResistance <= 0) return;

        Vector2 direction = (Vector2)transform.position - sourcePosition;
        enemyMovement.Knockback(knockbackForce * knockbackResistance * direction.normalized, KnockbackDuration);
    }
}
