using Photon.Pun;
using UnityEngine;

public class OilPowerup : Powerup
{
    public string OilProjectile;
    public Vector3 InitialVelocity = new Vector3(0,1,-1);
    public AudioClip OilSFX;

    public override void OnPickup(PowerupUser player)
    {
        base.OnPickup(player);
    }

    public override void Activate(PowerupUser player)
    {
        AudioSource.PlayClipAtPoint(OilSFX, player.transform.position, PlayerPrefs.GetFloat("SFXVolume", 1));
        GameObject projectile = PhotonNetwork.Instantiate(OilProjectile, player.transform.position, player.transform.rotation, 0);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        projectile.GetComponent<Collider>().enabled = false;
        rb.velocity = player.transform.TransformDirection(InitialVelocity) + player.GetComponent<Rigidbody>().velocity;
        player.ClearPowerup();
    }
}
