using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    private Sprite _icon;
    public Sprite Icon { get => _icon; private set => _icon = value; }

    [SerializeField]
    private string _characterName;
    public string CharacterName { get => _characterName; private set => _characterName = value; }

    [SerializeField]
    GameObject _baseWeapon;
    public GameObject BaseWeapon { get => _baseWeapon; private set => _baseWeapon = value; }

    [SerializeField]
    GameObject _basePassiveItem;
    public GameObject BasePassiveItem { get => _basePassiveItem; private set => _basePassiveItem = value; }

    [SerializeField]
    float _maxHealth;
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }

    [SerializeField]
    float _healthRegen;
    public float HealthRegen { get => _healthRegen; private set => _healthRegen = value; }

    [SerializeField]
    float _moveSpeed;
    public float MoveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }

    [SerializeField]
    float _might;
    public float Might { get => _might; private set => _might = value; }

    [SerializeField]
    float _magnetRadius;
    public float MagnetRadius { get => _magnetRadius; private set => _magnetRadius = value; }
}
