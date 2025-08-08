using UnityEngine;

public class TreasureChest : PickUps
{
    private InventoryManager inventory;

    private void Awake()
    {
        inventory = FindFirstObjectByType<InventoryManager>();
        IsMagnetizable = false;
    }

    protected override void OnCollected()
    {
        base.OnCollected();
        WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolutions()[Random.Range(0, inventory.GetPossibleEvolutions().Count)];

        if (inventory.TryExecuteEvolution(toEvolve))
        {
            Debug.Log("Эволюция запущена из сундука!");
        }
        else
        {
            Debug.LogWarning("Не удалось выполнить эволюцию");
        }
    }
}
