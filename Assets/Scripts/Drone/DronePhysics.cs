/*
** Author : Lucas BILLARD
** Source : Mario Haberle - Drone Tutoria Unity3d C# - https://www.youtube.com/playlist?list=PLPAgqhxd1Ib1YYqYnZioGyrSUzOwead17
** Date Create : 18/10/2022
** Description :
*   Class for simulate the physics of a drone using RelatveForce & Quaternion. It use axis Pitch, Roll, Yaw and Altitude to be accurate with a real drone
*   All the force and angle of the class are modifiable to be nearest from the reallity
*   This Script goal is to be use by controller's script, for autonomous or with device controller
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePhysics : MonoBehaviour
{
    private Rigidbody rb;

    public Pitch Pitch = Pitch.None;
    public Roll Roll = Roll.None;
    public Yaw Yaw = Yaw.None;
    public Altitude Altitude = Altitude.None;
    private float gravity = 9.81f;
    public float verticalPercentage;
    public float horizontalPercentage;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        MovementSideways();
        Rotation();
        Clampingvelocity();
        rb.AddRelativeForce(Vector3.up * upForce * rb.mass);
        rb.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, currentYrotation, tiltAmountSideways)
            );
    }

    public float upForce;

    private void MovementUpDown()
    {
        if (Pitch != Pitch.None || Roll != Roll.None)
        {
            if (Altitude != Altitude.None)
            {
                rb.velocity = rb.velocity;
            }
            else if (Yaw == Yaw.None)
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, Time.deltaTime * 5), rb.velocity.z);
                upForce = gravity * 3;
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0, Time.deltaTime * 5), rb.velocity.z);
                upForce = gravity + 1;
            }

            if (Yaw != Yaw.None)
            {
                upForce = gravity * 4;
            }
        }

        if (Roll != Roll.None && Pitch == Pitch.None)
        {
            upForce = gravity * 2;
        }
        if (Altitude == Altitude.Up)
        {
            upForce = gravity * 4;
            if (Roll != Roll.None)
            {
                upForce = gravity * 5;
            }
        }
        else if (Altitude == Altitude.Down)
        {
            upForce = -gravity * 2;
        }
        else if (Pitch == Pitch.None && Roll == Roll.None)
        {
            upForce = gravity;
        }
    }

    public float forwardSpeed = 50.0f;
    public float forwardAngle = 20;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;

    private void MovementForward()
    {
        int orientation = Pitch == Pitch.Forward ? 1 : Pitch == Pitch.Backward ? -1 : 0;
        rb.AddRelativeForce(Vector3.forward * orientation * forwardSpeed);
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, forwardAngle * orientation, ref tiltVelocityForward, 0.5f);
    }

    public float rotationByKeys = 0;
    private float currentYrotation;
    private float wantedYrotation = 20;
    private float rotationVelocity;

    private void Rotation()
    {
        int orientation = Yaw == Yaw.Hours ? 1 : Yaw == Yaw.AntiHours ? -1 : 0;
        wantedYrotation += orientation * rotationByKeys;
        currentYrotation = Mathf.SmoothDamp(currentYrotation, wantedYrotation, ref rotationVelocity, 0.25f);
    }

    private Vector3 velocityToSmoothZero;

    private void Clampingvelocity()
    {
        if (Pitch != Pitch.None)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10.0f, Time.deltaTime! + 1f));
        }
        else if (Roll != Roll.None)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, Mathf.Lerp(rb.velocity.magnitude, 10.0f, Time.deltaTime! + 1f));
        }
        else
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref velocityToSmoothZero, 0.95f);
        }
    }

    public float sidewaysdSpeed = 30.0f;
    public float sidewaysAngle = 20;
    private float tiltAmountSideways = 0;
    private float tiltVelocitySideways;

    private void MovementSideways()
    {
        int orientation = Roll == Roll.Right ? 1 : Roll == Roll.Left ? -1 : 0;
        rb.AddRelativeForce(Vector3.right * orientation * sidewaysdSpeed);
        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -1 * sidewaysAngle * orientation, ref tiltVelocitySideways, 0.3f);
    }
}