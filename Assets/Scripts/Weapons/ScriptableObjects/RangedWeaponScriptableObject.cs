using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/RangedWeapon")]
public class RangedWeaponScriptableObject : WeaponScriptableObject
{
    [SerializeField]
    private float _projectileSpeed;
    public float ProjectileSpeed { get => _projectileSpeed; private set => _projectileSpeed = value; }

    [SerializeField]
    private int _pierce;
    public int Pierce { get => _pierce; private set => _pierce = value; }
}
