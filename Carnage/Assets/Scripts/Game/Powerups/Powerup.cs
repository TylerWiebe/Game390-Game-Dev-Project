using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public abstract class Powerup: MonoBehaviour
{
    public Sprite PowerUpIcon;
    

    public virtual void OnPickup(PowerupUser player)
    {
        //if we need the default callback
        
    }

    public abstract void Activate(PowerupUser player);
}