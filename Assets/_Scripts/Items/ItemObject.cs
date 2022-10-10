using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Basic Item")]
public class ItemObject : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public int value;
}
