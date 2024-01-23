using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(long Damage, DamageType DamageTypeValue);

    void FloatingDamage(Vector3 position, long Damage, DamageType DamageTypeValue);
}
