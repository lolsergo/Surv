using System;
using System.Collections.Generic;

[System.Serializable]
public class ProfileData
{
    public string profileName;
    public int totalGold;
    public List<string> boughtUpgrades;
    public DateTime creationDate;
    public DateTime lastPlayed;

    public ProfileData(string name)
    {
        profileName = name;
        totalGold = 0;
        boughtUpgrades = new List<string>();
        creationDate = DateTime.Now;
        lastPlayed = DateTime.Now;
    }
}
