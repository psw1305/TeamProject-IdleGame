using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(long damage, DamageType damageTypeValue);

    void FloatingDamage(Vector3 position, long damage, DamageType damageTypeValue);
}
