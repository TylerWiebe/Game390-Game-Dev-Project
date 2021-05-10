using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float EffectTime = 5;
    public int MaxTime = 1000;

    private int count = 0;
    private void Update() {
        count++;
        if (count >= MaxTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        PlayerCarController player = other.GetComponent<PlayerCarController>();
        StatusEffectManager manager = other.GetComponent<StatusEffectManager>();
        if (player != null)
            manager.ApplyEffect(new SpikeTrapEffect(player), EffectTime);
    }
}
