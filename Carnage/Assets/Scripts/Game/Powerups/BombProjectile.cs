using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;

public class BombProjectile : MonoBehaviour
{
    public int FuseLength = 200;
    public float MaxExplosionSize = 20;
    public float ExplosionSpeed = 1;
    public float ExplosionForce = 30;
    private int count = 0;

    public AudioClip ExplosionSFX;
    public AudioMixer MyMixer;
    
    void Update()
    {
        count++;
        if (count == 50) {
            GetComponent<Collider>().enabled = true;
        }
        if (count == FuseLength) {
            GetComponent<Collider>().enabled = true;
            Explode();
        }
    }

    private void Explode() {
        PlayExplosionSFXAt(ExplosionSFX, gameObject.transform.position, PlayerPrefs.GetFloat("SFXVolume", 1));
        Explosion explosion = PhotonNetwork.Instantiate("Explosion Force", transform.position, transform.rotation, 0).GetComponent<Explosion>();
        explosion.Speed = ExplosionSpeed;
        explosion.MaxSize = MaxExplosionSize;
        explosion.Force = ExplosionForce;
        Destroy(gameObject);
    }

    private AudioSource PlayExplosionSFXAt(AudioClip AC, Vector3 pos, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = AC; // define the clip
        aSource.outputAudioMixerGroup = MyMixer.FindMatchingGroups("Master/SFX/Explosion")[0];
        aSource.volume = volume;
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, AC.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }
}
