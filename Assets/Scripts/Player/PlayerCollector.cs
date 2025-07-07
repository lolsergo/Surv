using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out iCollectible collectible))
        {
            collectible.Collect();
        }
    }
}
