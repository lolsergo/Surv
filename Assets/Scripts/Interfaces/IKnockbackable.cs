using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(Vector2 sourcePosition, float knockbackForce, float knockbackDuration);
}