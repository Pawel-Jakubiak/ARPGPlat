using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> _inventoryData;

    public static InventorySystem Instance;

    public List<InventorySlot> GetInventoryData { get { return _inventoryData; } }

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
            _inventoryData.Add(slot);

            return;
        }

        slot.Add(amount);
    }

    public InventorySlot GetInventoryItem(ItemObject item)
    {
        foreach (InventorySlot invItem in _inventoryData)
            if (invItem.item == item)
                return invItem;

        return null;
    }
}
