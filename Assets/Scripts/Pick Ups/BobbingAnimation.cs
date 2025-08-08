using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    private Vector3 initialPosition;
    private bool isBeingPulled;
    private Rigidbody2D rb;

    private void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>(); // Кэшируем Rigidbody2D
    }

    private void Update()
    {
        if (!isBeingPulled)
        {
            // Только если предмет не притягивается
            transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }

    public void StartPull()
    {
        isBeingPulled = true;
    }
}