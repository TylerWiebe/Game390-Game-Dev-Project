using UnityEngine;

public class Beacon : MonoBehaviour
{
    public float minDistance = 30;
    public float maxDistance = 100;
    private SpriteRenderer sprite;
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        Color color = sprite.color;
        float alpha = 1;
        float distance = maxDistance;
        if (MultiplayerPlayer.LocalPlayer != null) {
            distance = Mathf.Abs(Vector3.Distance(MultiplayerPlayer.LocalPlayer.transform.position, transform.position));
        }
        if (distance <= minDistance) {
            alpha = 0;
        }
        else if (distance >= maxDistance) {
            alpha = 1;
        }
        else {
            alpha = (distance-minDistance)/(maxDistance-minDistance);
        }
        color.a = alpha;
        sprite.color = color;
    }
}
