using UnityEngine;

public class PickUps : MonoBehaviour, ICollectible
{
    protected bool hasBeenCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        OnCollected();
        Destroy(gameObject);
    }

    protected virtual void OnCollected()
    {
        hasBeenCollected = true;
        Debug.Log($"{this.name} подобран!");
    }
}
