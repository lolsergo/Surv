using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    int expPerKill;
    public int ExpPerKill { get => expPerKill; private set => expPerKill = value; }

    [SerializeField]
    int spawnRadius;
    public int SpawnRadius { get => spawnRadius; private set => spawnRadius = value; }

    [SerializeField]
    int despawnDistance;
    public int DespawnDistance { get => despawnDistance; private set => despawnDistance = value; }

    [SerializeField]
    float _knockbackResistance;
    public float KnockbackResistance { get => _knockbackResistance; private set => _knockbackResistance = value; }
}
