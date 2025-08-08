using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MetaProgressData
{
    public int TotalGold = 0;                       // Накопленное золото
    public List<string> BoughtUpgrades = new();     // Купленные улучшения (их ID)
}
