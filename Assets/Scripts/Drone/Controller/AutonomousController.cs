using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousController : MonoBehaviour
{
    public float AltitudeToReach;
    public float ForwardToReach;
    public float LeftToReach;
    public float appro;
    private DronePhysics drone;

    // Start is called before the first frame update
    private void Awake()
    {
        drone = GetComponent<DronePhysics>();
    }

    private int downPress = 0;

    private void FixedUpdate()
    {
        var upforce = drone.upForce;
        Debug.Log($"Drone Position = {drone.transform.position}, Drone RBPosition '={drone.rb.position}");

        if (AltitudeToReach - appro > drone.transform.position.y)
        {
            drone.Altitude = Altitude.Up;
        }
        else if (downPress < 3)
        {
            drone.Altitude = Altitude.Down;
            downPress++;
        }
        else
        {
            drone.Altitude = Altitude.None;
        }
    }
}