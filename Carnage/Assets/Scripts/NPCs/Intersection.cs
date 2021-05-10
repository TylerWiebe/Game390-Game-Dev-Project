using UnityEngine;

public class Intersection : MonoBehaviour
{
    public NPCCarController currentCar = null;
    public NPCCarController prevCar = null;

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTrigger(other);
    }

    private void OnTrigger(Collider other)
    {
        Transform root = other.transform.root;
        if (!root.CompareTag(Tags.Player) || other.name == "BrakeTrigger")
            return;

        NPCCarController npc = root.GetComponent<NPCCarController>();

        if (npc == null)
            return;

        if (currentCar == null)
        {
            currentCar = npc;
        }

        if (npc == currentCar || npc == prevCar)
        {
            npc.ShouldBrake(false);
        }
        else
        {
            npc.ShouldBrake(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform root = other.transform.root;
        if (!root.CompareTag(Tags.Player) || other.name == "BrakeTrigger")
            return;

        NPCCarController npc = root.GetComponent<NPCCarController>();

        if (npc == null)
            return;

        if (npc == currentCar)
        {
            prevCar = currentCar;
            currentCar = null;
        }
    }

    private void Update()
    {
        if (currentCar == null)
            return;

        // Fallback in case somehow OnTriggerExit does not fire
        if ((currentCar.transform.position - transform.position).magnitude > 6f * transform.localScale.x)
        {
            Debug.LogError("Fallback intersection exit triggered");
            prevCar = currentCar;
            currentCar = null;
        }
    }
}
