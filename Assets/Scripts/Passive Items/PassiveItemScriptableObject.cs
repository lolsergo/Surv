using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/PassiveItems")]
public class PassiveItemScriptableObject : InventoryItemScriptableObject
{
    [SerializeField]
   private float multipler;
    public float Multipler { get => multipler; private set => multipler = value; } // in percent
}
