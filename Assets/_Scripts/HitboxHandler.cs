using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHandler : MonoBehaviour
{
    [SerializeField] private IDamageSource _source;
    [SerializeField] private List<Collider> _collidersAlreadyHit;

    private void Start()
    {
        _source = GetComponentInParent<IDamageSource>();
    }

    private void OnEnable()
    {
        _collidersAlreadyHit = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag)) 
            return;

        if (_collidersAlreadyHit.Contains(other)) 
            return;

        _collidersAlreadyHit.Add(other);

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable == null) 
            return;

        DamageInfo damageInfo = new DamageInfo(_source, _source.CalculateDamage());

        damageable.OnHit(damageInfo);
    }
}
