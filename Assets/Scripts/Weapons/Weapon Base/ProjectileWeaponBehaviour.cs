using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSecons;
    public bool applyRotation = true;
    public float angleOffset = 0f;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecons);

        if (applyRotation && direction != Vector3.zero)
        {
            RotateTowardsDirection();
        }
    }

    //public void DirectionChecker(Vector2 dir)
    //{
    //    direction = dir.normalized;

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //    transform.rotation = Quaternion.Euler(0, 0, angle);

    //    //float dirX = direction.x;
    //    //float dirY = direction.y;

    //    //Vector3 scale = transform.localScale;
    //    //Vector3 rotation = transform.rotation.eulerAngles;

    //    //if (dirX < 0 && dirY == 0)
    //    //{
    //    //    scale.x *= -1;
    //    //    scale.y *= -1;
    //    //}

    //    //transform.localScale = scale;
    //    //transform.rotation = Quaternion.Euler(rotation);
    //}

    public void DirectionChecker(Vector2 dir)
    {
        direction = dir.normalized;
        if (applyRotation) RotateTowardsDirection();
    }

    private void RotateTowardsDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += angleOffset; // Применяем сдвиг
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
