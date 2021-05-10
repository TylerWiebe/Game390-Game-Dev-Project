using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public List<NavPoint> Points;

    public NavPoint Next(NavPoint prevPoint)
    {
        List<NavPoint> availablePoints = new List<NavPoint>(Points);
        availablePoints.Remove(prevPoint);

        int idx = 0;
        while (idx < availablePoints.Count)
        {
            if (availablePoints[idx] == null || !availablePoints[idx].gameObject.activeInHierarchy)
            {
                availablePoints.RemoveAt(idx);
            }
            else
            {
                idx++;
            }
        }

        if (availablePoints.Count == 0)
            return prevPoint;

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    public NavPoint Next()
    {
        return Next(null);
    }

    public void OnTriggerEnter(Collider other)
    {
        OnTrigger(other);
    }

    public void OnTriggerStay(Collider other)
    {
        OnTrigger(other);
    }

    private void OnTrigger(Collider other)
    {
        Transform root = other.transform.root;
        if (root.gameObject.layer == 9 && other.name == "BrakeTrigger")
            return;

        Navigator navigator = root.GetComponent<Navigator>();

        if (navigator == null || navigator.TargetPoint != this)
            return;

        navigator.TargetPoint = Next(navigator.PreviousPoint);
        navigator.PreviousPoint = this;
    }
}
