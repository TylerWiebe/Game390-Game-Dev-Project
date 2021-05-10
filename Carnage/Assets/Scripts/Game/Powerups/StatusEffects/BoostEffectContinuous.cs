using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffectContinuous : BoostEffect
{
    public BoostEffectContinuous(PlayerCarController player) : base(player) { }
    protected override void OnActivate()
    {
        player.isBoost = true;
    }

    protected override void OnDeactivate()
    {
        player.isBoost = false;
    }
}
