using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    [HideInInspector]
    public float Distance;

    public List<string> NoHittingTags;

    public Color HitColor = Color.yellow;
    public Color NotHitColor = Color.white;

    // Update is called once per frame
    private void Update()
    {
        Distance = GetSensorData();
    }

    private float GetSensorData()
    {
        RaycastHit hit;
        bool hitting = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity);

        if (hitting && !NoHittingTags.Contains(hit.transform.tag))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, HitColor);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, NotHitColor);

            hit.distance = float.MaxValue;
        }

        return hit.distance;
    }
}