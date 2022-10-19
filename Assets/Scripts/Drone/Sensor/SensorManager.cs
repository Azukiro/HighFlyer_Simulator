using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
    private DistanceSensor[] distanceSensors;

    // Start is called before the first frame update
    private void Awake()
    {
        distanceSensors = GetComponentsInChildren<DistanceSensor>();
        Debug.Log(distanceSensors.Length);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}