using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDrone : MonoBehaviour
{
    private DroneController controller;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<DroneController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Z");
            controller.Pitch = Pitch.Forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            controller.Pitch = Pitch.Backward;
        }
        else
        {
            controller.Pitch = Pitch.None;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            controller.Roll = Roll.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            controller.Roll = Roll.Right;
        }
        else
        {
            controller.Roll = Roll.None;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            controller.Altitude = Altitude.Up;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Altitude = Altitude.Down;
        }
        else
        {
            controller.Altitude = Altitude.None;
        }
    }
}