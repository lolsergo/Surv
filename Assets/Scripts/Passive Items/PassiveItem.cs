using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected CurrentPlayerStats currentPlayerStats;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {

    }

    void Start()
    {
        currentPlayerStats = FindFirstObjectByType<CurrentPlayerStats>();
        ApplyModifier();
    }
}
