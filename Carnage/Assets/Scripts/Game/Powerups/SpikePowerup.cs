using Photon.Pun;
using UnityEngine;

public class SpikePowerup : Powerup
{
    public string SpikeProjectile;
    public Vector3 InitialVelocity = new Vector3(0,1,-1);
    public AudioClip SpikeSFX;

    public override void OnPickup(PowerupUser player)
    {
        base.OnPickup(player);
    }

    public override void Activate(PowerupUser player)
    {
        AudioSource.PlayClipAtPoint(SpikeSFX, player.transform.position, PlayerPrefs.GetFloat("SFXVolume", 1));
        GameObject projectile = PhotonNetwork.Instantiate(SpikeProjectile, player.transform.position + new Vector3(0, 2f, 0), player.transform.rotation, 0);
        projectile.GetComponent<Rigidbody>().velocity = player.transform.TransformDirection(InitialVelocity) + player.GetComponent<Rigidbody>().velocity;
        player.ClearPowerup();
    }
}
