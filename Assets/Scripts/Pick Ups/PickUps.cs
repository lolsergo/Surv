using UnityEngine;

public class PickUps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (TryGetComponent(out ICollectible collectible))
            {
                collectible.Collect();
            }
            Destroy(gameObject);
        }
    }
}
