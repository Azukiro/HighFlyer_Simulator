using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LaserSensor : DistanceSensor
{
    // Update is called once per frame
    private void FixedUpdate()
    {
        Distance = GetSensorData();
    }

    protected override float GetSensorData()
    {
        RaycastHit hit;
        bool hitting = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity);

        if (hitting && CollisionObjectIsValid(hit.transform.tag))
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