using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float MaxSize = 20;
    public float Speed = 1;
    public float Force = 30;

    void Update()
    {
        transform.localScale += new Vector3(Speed, Speed, Speed);
        if (transform.localScale.x > MaxSize) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        other.attachedRigidbody.AddExplosionForce(Force, transform.position, transform.localScale.x, 3, ForceMode.VelocityChange);    
    }
}
