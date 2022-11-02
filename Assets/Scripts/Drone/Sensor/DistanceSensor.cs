using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DistanceSensor : MonoBehaviour
{
    [HideInInspector]
    public float Distance;

    [SerializeField]
    protected List<string> NoHittingTags;

    [SerializeField]
    protected Color HitColor = Color.yellow;

    [SerializeField] protected Color NotHitColor = Color.white;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Distance = GetSensorData();
    }

    protected abstract float GetSensorData();

    protected bool CollisionObjectIsValid(string tag)
    {
        return !NoHittingTags.Contains(tag);
    }

    public bool HitSomething()
    {
        return Distance != float.MaxValue;
    }

    public bool NearDistance(float distanceToReach)
    {
        return HitSomething() && Distance.CompareToWithApproximation(distanceToReach, 1f) != 0;
    }
}