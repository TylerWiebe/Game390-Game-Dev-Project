using UnityEngine;

public class DebugPowerup : Powerup
{
    public override void OnPickup(PowerupUser player)
    {
        base.OnPickup(player);
        Debug.Log("Player picked up debug powerup item.");
    }
    public override void Activate(PowerupUser player)
    {
        Debug.Log("Player activated debug powerup item.");
        player.ClearPowerup();
    }
}
