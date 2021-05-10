using UnityEngine;

public class BoostPowerupContinuous : BoostPowerup
{
    public AudioClip BoostContSFX;
    
    public override void Activate(PowerupUser player)
    {

        AudioSource.PlayClipAtPoint(BoostContSFX, player.transform.position, PlayerPrefs.GetFloat("SFXVolume", 1));
        base.Activate(player);
        PlayerCarController pcc = player.GetComponent<PlayerCarController>();
        StatusEffectManager manager = player.GetComponent<StatusEffectManager>();
        manager.ApplyEffect(new BoostEffectContinuous(pcc), EffectTime);
    }
}

