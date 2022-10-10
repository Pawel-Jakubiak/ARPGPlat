using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponObject : ItemObject
{
    public WeaponType type;
    public MinMax damage;
}

public enum WeaponType
{
    SWORD = 0,
    AXE = 1,
    CLUB = 2
}