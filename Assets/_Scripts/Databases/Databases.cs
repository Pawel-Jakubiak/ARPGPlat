using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Databases : MonoBehaviour
{
    private static Databases _instance;

    [SerializeField] private ItemDatabase _items;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public static ItemObject GetItemById(string id)
    {
        ItemObject foundItem = _instance._items.itemDatabase.Find(i => i.id == id);

        return foundItem;
    }
}
