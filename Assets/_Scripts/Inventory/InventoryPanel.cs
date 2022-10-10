using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject _inventorySlotObject;
    [SerializeField] private Transform _viewportContent;

    private List<GameObject> _instantiatedSlots = new List<GameObject>();

    private void Start()
    {
    }

    private void OnEnable()
    {
        /*_inventoryData = InventorySystem.Instance.GetInventoryData;

        foreach (InventorySlot slot in _inventoryData.items)
        {
            GameObject newSlot = Instantiate(_inventorySlotObject, _viewportContent);
            newSlot.GetComponent<TextMeshProUGUI>().SetText(slot.item.displayName);

        }*/
    }
}