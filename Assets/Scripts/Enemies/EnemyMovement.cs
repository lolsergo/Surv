using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    EnemyStats enemyStats;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = FindFirstObjectByType<PlayerMovement>().transform;
    }

    void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemyStats.currentMoveSpeed * Time.deltaTime);
        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
