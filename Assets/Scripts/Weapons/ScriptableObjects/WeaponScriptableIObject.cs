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
}
