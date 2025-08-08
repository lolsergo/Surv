using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerController playerStats;
    private CircleCollider2D magnetCollector;

    public float pullSpeed;

    private void Start()
    {
        playerStats = FindFirstObjectByType<PlayerController>();
        magnetCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        magnetCollector.radius = playerStats.CurrentMagnetRadius.Value;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.TryGetComponent(out ICollectible collectible) || !collectible.IsMagnetizable)
            return;

        if (!collider.TryGetComponent(out Rigidbody2D rigidBody))
        {
            Debug.LogWarning($"Объект {collider.name} имеет ICollectible, но не имеет Rigidbody2D!");
            return;
        }

        if (collider.TryGetComponent(out BobbingAnimation bobbing))
            bobbing.StartPull();

        Vector2 forceDirection = (transform.position - collider.transform.position).normalized;
        rigidBody.AddForce(forceDirection * pullSpeed, ForceMode2D.Impulse); // ForceMode2D.Impulse для резкого рывка
    }
}
