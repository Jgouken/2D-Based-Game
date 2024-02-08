using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file is special, as it is cannot be applied to an individual object.
// Instead, it can be called by any other file as a sort of custom global function.

[System.Serializable]
public class Cooldown
{
    [SerializeField] private float cooldownTime;
    private float _nextTeleTime;

    public bool IsCoolingDown => Time.time < _nextTeleTime;
    public void StartCooldown() => _nextTeleTime = Time.time + cooldownTime;
}
