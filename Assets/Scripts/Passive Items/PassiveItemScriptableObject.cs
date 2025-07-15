using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/PassiveItems")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
   private float multipler;
    public float Multipler { get => multipler; private set => multipler = value; } // in percent

    [SerializeField]
    private int _upgradableItemLevel;
    public int UpgradableItemLevel { get => _upgradableItemLevel; private set => _upgradableItemLevel = value; }

    [SerializeField]
    private GameObject _nextLevelPrefab;
    public GameObject NextLevelPrefab { get => _nextLevelPrefab; private set => _nextLevelPrefab = value; }

    [SerializeField]
    private string _passiveItemName;
    public string PassiveItemName { get => _passiveItemName; private set => _passiveItemName = value; }
}
