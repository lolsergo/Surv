using UnityEngine;

public abstract class WeaponScriptableObject : InventoryItemScriptableObject
{
    [SerializeField]
    private GameObject _prefab;
    public GameObject Prefab { get => _prefab; private set => _prefab = value; }

    [SerializeField]
    private float _damage;
    public float Damage { get => _damage; private set => _damage = value; }

    [SerializeField]
    private float _cooldownDuration;
    public float CooldownDuration { get => _cooldownDuration; private set => _cooldownDuration = value; }

    [SerializeField]
    private float _knockbackForce;
    public float KnockbackForce { get => _knockbackForce; private set => _knockbackForce = value; }

    [SerializeField]
    private float _knockbackDuration;
    public float KnockbackDuration { get => _knockbackDuration; private set => _knockbackDuration = value; }
}
