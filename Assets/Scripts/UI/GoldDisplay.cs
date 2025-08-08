using TMPro;
using UnityEngine;

public abstract class GoldDisplay : MonoBehaviour
{
    [SerializeField] protected TMP_Text goldText;

    public virtual void UpdateDisplay(int amount)
    {
        goldText.text = $"Gold: {amount}";
    }
}
