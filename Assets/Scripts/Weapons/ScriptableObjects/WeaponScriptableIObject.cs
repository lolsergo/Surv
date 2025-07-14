using UnityEngine;

public class WeaponScriptableObject : ScriptableObject
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
    private int _upgradableItemLevel;
    public int UpgradableItemLevel { get => _upgradableItemLevel; private set => _upgradableItemLevel = value; }

    [SerializeField]
    private GameObject _nextLevelPrefab;
    public GameObject NextLevelPrefab { get => _nextLevelPrefab; private set => _nextLevelPrefab = value; }
}
