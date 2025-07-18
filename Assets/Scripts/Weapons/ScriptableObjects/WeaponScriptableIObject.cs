using UnityEngine;

public abstract class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    private string _weaponName;
    public string WeaponName { get => _weaponName; private set => _weaponName = value; }

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

    [SerializeField]
    Sprite _icon;
    public Sprite Icon { get => _icon; private set => _icon = value; }
}
