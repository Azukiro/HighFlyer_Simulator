/*
** Author : Lucas BILLARD
** Date Create : 17/10/2022
** Description :
*   Class for control the drone with Keyboard using DronePhysics class
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Obsolete]
public class KeyboardController : MonoBehaviour
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
        if (Input.GetKey(KeyCode.Z))
        {
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

        if (Input.GetKey(KeyCode.E))
        {
            controller.Yaw = Yaw.Hours;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            controller.Yaw = Yaw.AntiHours;
        }
        else
        {
            controller.Yaw = Yaw.None;
        }
    }
}