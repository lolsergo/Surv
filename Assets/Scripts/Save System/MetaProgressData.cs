using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MetaProgressData
{
    public int TotalGold = 0;                       // ����������� ������
    public List<string> BoughtUpgrades = new();     // ��������� ��������� (�� ID)
}
