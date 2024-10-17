using System;
using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(int damage, AttackAndTextType attackType, Action criticalEvent);
}
