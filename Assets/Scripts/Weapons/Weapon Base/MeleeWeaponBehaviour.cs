using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public float destroyAfterSecons;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSecons);
    }

    void Update()
    {
        
    }
}
