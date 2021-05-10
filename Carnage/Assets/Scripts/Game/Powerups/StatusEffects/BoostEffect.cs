using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : StatusEffect
{
    private float MaxAcclerationMultiplier = 100.0f;
    private float MaxSpeedMultiplier = 2.0f;

    public BoostEffect(PlayerCarController player) : base(player) { }

    protected override void OnActivate()
    {
        player.MaxAccleration *= MaxAcclerationMultiplier;
        player.MaxSpeed *= MaxSpeedMultiplier;
    }

    protected override void OnDeactivate()
    {
        player.MaxAccleration /= MaxAcclerationMultiplier;
        player.MaxSpeed /= MaxSpeedMultiplier;
    }
}
