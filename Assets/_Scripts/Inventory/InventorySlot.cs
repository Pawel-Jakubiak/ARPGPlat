using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Add(int amount) => quantity += amount;

    public void Remove(int amount) => quantity -= amount;
}
