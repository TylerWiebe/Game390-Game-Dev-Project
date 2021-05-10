using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapEffect : StatusEffect
{
    private float TurnResitrictionMultiplier = 0.25f;
    private float MaxSpeedMultiplier = 0.75f;

    public SpikeTrapEffect(PlayerCarController player) : base(player) {}

    protected override void OnActivate() {
        player.MaxSteeringAngle *= TurnResitrictionMultiplier;
        player.MaxSpeed *= MaxSpeedMultiplier;
    }

    protected override void OnDeactivate() {
        player.MaxSteeringAngle /= TurnResitrictionMultiplier;
        player.MaxSpeed /= MaxSpeedMultiplier;
    }
}
