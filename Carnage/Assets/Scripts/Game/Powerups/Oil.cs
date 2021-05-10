using UnityEngine;

public class Oil : MonoBehaviour
{

    public float Slipperyness = 2;
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
        WheelFrictionCurve ff = player.ForwardWheelFriction;
        WheelFrictionCurve sf = player.SideWheelFriction;
        ff.stiffness = ff.stiffness/Slipperyness;
        sf.stiffness = sf.stiffness/Slipperyness;
        player.ForwardWheelFriction = ff;
        player.SideWheelFriction = sf;
    }

    private void OnTriggerExit(Collider other) {
        PlayerCarController player = other.GetComponent<PlayerCarController>();
        WheelFrictionCurve ff = player.ForwardWheelFriction;
        WheelFrictionCurve sf = player.SideWheelFriction;
        ff.stiffness = ff.stiffness*Slipperyness;
        sf.stiffness = sf.stiffness*Slipperyness;
        player.ForwardWheelFriction = ff;
        player.SideWheelFriction = sf;
    }
}
