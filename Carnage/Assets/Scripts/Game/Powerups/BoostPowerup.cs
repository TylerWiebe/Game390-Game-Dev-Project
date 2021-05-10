using System.Threading.Tasks;
using UnityEngine;

public class BoostPowerup : Powerup
{
    public float AcclerationMultiplier = 100.0f;
    public float SpeedMultiplier = 2.0f;
    public float EffectTime = 5;
    public AudioClip BoostSFX;
    
    public override void OnPickup(PowerupUser player)
    {
        base.OnPickup(player);
    }

    public override void Activate(PowerupUser player)
    {
        AudioSource.PlayClipAtPoint(BoostSFX, player.transform.position, PlayerPrefs.GetFloat("SFXVolume", 1));
        PlayerCarController pcc = player.GetComponent<PlayerCarController>();
        StatusEffectManager manager = player.GetComponent<StatusEffectManager>();
        manager.ApplyEffect(new BoostEffect(pcc), EffectTime);
        player.ClearPowerup();
    }

}
