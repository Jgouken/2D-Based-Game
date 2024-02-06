using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    [SerializeField] private float cooldownTime;
    private float _nextTeleTime;

    public bool IsCoolingDown => Time.time < _nextTeleTime;
    public void StartCooldown() => _nextTeleTime = Time.time + cooldownTime;
}
