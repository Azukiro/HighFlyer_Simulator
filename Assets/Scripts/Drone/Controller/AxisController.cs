/*
** Author : Lucas BILLARD
** Date Create : 17/10/2022
** Description :
*   Class for control the drone with Keyboard using DronePhysics class
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisController : MonoBehaviour
{
    private DronePhysics controller;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<DronePhysics>();
    }

    // Update is called once per frame
    private void Update()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        var altitude = Input.GetAxis("Altitude");

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