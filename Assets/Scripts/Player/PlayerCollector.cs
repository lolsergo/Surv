using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerController playerStats;
    CircleCollider2D magnetCollector;
    
    public float pullSpeed;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerController>();
        magnetCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        magnetCollector.radius = playerStats.CurrentMagnetRadius.Value;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out ICollectible collectible))
        {
            Rigidbody2D rigidBody = collider.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - collider.transform.position).normalized;
            rigidBody.AddForce(forceDirection * pullSpeed);
        }
    }
}
