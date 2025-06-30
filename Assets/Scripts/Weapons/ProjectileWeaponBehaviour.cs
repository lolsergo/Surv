using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSecons;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecons);
    }

    public void DirectionChecker(Vector2 dir)
    {
        direction = dir;
    }
}
