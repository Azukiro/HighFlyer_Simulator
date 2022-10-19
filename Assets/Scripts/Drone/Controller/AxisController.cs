/*
** Author : Lucas BILLARD
** Date Create : 17/10/2022
** Description :
*   Class for control the drone with Keyboard using DronePhysics class
*/

using UnityEngine;

public class AxisController : MonoBehaviour
{
    DronePhysics controller;

    private void Start()
    {
        controller = GetComponent<DronePhysics>();
    }

    private void Update()
    {
        bool arm, takeOff, land, desarm;
        arm     = Input.GetButtonDown("Arm");
        takeOff = Input.GetButtonDown("Take off");
        land    = Input.GetButtonDown("Land");
        desarm  = Input.GetButtonDown("Desarm");

        switch (controller.state)
        {
            case (DroneState.OFF):
                if (arm)
                    controller.Arm();
                break;
            case (DroneState.WAITING_FLY_INSTRUCTIONS_ON_GROUND):
                if (takeOff)
                    controller.TakeOff();
                if (desarm)
                    controller.Desarm();
                break;
            case (DroneState.FLYING):
                if (land)
                    controller.Land();
                else
                    ControlDroneFlight();
                break;
        }
    }

    private void ControlDroneFlight()
    {
        float vertical, horizontal, altitude;

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        altitude = Input.GetAxis("Altitude");

        var yaw = Input.GetAxis("Yaw");
        if (vertical > 0)
        {
            controller.Pitch = Pitch.Forward;
        }
        else if (vertical < 0)
        {
            controller.Pitch = Pitch.Backward;
        }
        else
        {
            controller.Pitch = Pitch.None;
        }
        controller.verticalPercentage = vertical;

        if (horizontal < 0)
        {
            controller.Roll = Roll.Left;
        }
        else if (horizontal > 0)
        {
            controller.Roll = Roll.Right;
        }
        else
        {
            controller.Roll = Roll.None;
        }
        controller.verticalPercentage = horizontal;

        if (altitude > 0)
        {
            controller.Altitude = Altitude.Up;
        }
        else if (altitude < 0)
        {
            controller.Altitude = Altitude.Down;
        }
        else
        {
            controller.Altitude = Altitude.None;
        }

        if (yaw > 0)
        {
            controller.Yaw = Yaw.Hours;
        }
        else if (yaw < 0)
        {
            controller.Yaw = Yaw.AntiHours;
        }
        else
        {
            controller.Yaw = Yaw.None;
        }
    }
}