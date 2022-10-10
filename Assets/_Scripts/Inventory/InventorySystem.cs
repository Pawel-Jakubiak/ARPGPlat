using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    /*[SerializeField] private InventoryData _inventoryData;

    public static InventorySystem Instance;

    public InventoryData GetInventoryData { get { return _inventoryData; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log($"H{_inventoryData}");
    }

    public void AddToInventory(ItemObject item, int amount = 1)
    {
        InventorySlot slot = GetInventoryItem(item);

        if (slot == null)
        {
            slot = new InventorySlot(item, amount);
            _inventoryData.items.Add(slot);

            return;
        }

        slot.Add(amount);
    }

    public InventorySlot GetInventoryItem(ItemObject item)
    {
        foreach (InventorySlot invItem in _inventoryData.items)
            if (invItem.item == item)
                return invItem;

        return null;
    }*/
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int quantity;

    public InventorySlot(ItemObject item, int amount)
    {
        this.item = item;
        quantity = amount;
    }

    public void Add(int amount) => this.quantity += amount;

    public void Remove(int amount) => this.quantity -= amount;
}
