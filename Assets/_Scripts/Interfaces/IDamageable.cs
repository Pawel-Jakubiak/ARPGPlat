using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnHit(DamageInfo damageInfo);

    public GameObject gameObject { get; }
}