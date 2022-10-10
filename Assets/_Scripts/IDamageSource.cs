using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageSource
{
    int CalculateDamage();
}

public struct DamageInfo
{
    // todo type of dmg etc.
    public IDamageSource source;
    public int damage;

    public DamageInfo(IDamageSource _source, int _damage)
    {
        source = _source;
        damage = _damage;
    }
}
