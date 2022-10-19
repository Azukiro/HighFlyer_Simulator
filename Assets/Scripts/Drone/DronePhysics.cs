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

public enum DroneState { 
    OFF, 
    ACTIVATING_ROTORS, 
    WAITING_FLY_INSTRUCTIONS_ON_GROUND, 
    TAKING_OFF, 
    FLYING, 
    LANDING, 
    DESACTIVATING_ROTORS 
}

public enum Pitch
{
    None,
    Backward,
    Forward
}

public enum Roll
{
    None,
    Left,
    Right
}

public enum Yaw
{
    None,
    Hours,
    AntiHours
}

public enum Altitude
{
    None,
    Up,
    Down
}

public class DronePhysics : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;
    private Rotor[] rotors;

    [HideInInspector] public Pitch Pitch = Pitch.None;
    [HideInInspector] public Roll Roll = Roll.None;
    [HideInInspector] public Yaw Yaw = Yaw.None;
    [HideInInspector] public Altitude Altitude = Altitude.None;
    [HideInInspector] private float gravity = 9.81f;
    [HideInInspector] public float verticalPercentage;
    [HideInInspector] public float horizontalPercentage;


    [Header("Altitude")]
    public float upForce;

    [Header("Pitch")]
    public float forwardSpeed = 50.0f;
    public float forwardAngle = 20;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;

    [Header("Yaw")]
    public float rotationByKeys = 0;
    private float currentYrotation;
    private float wantedYrotation = 20;
    private float rotationVelocity;
    private Vector3 velocityToSmoothZero;

    [Header("Roll")]
    public float sidewaysdSpeed = 30.0f;
    public float sidewaysAngle = 20;
    private float tiltAmountSideways = 0;
    private float tiltVelocitySideways;

    [Header("Take off")]
    public float takeOffTime = 1f;


    [HideInInspector]
    private bool IsGrounded = false;

    [HideInInspector]
    public DroneState state = DroneState.OFF;

    #endregion


    private void Awake()
    {
        rb     = GetComponent<Rigidbody>();
        rotors = GetComponentsInChildren<Rotor>();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case DroneState.OFF:
                return;
            case DroneState.ACTIVATING_ROTORS:
                ChangeStateIfAllRotorsAreFullyArmed();
                return;
            case DroneState.WAITING_FLY_INSTRUCTIONS_ON_GROUND:
                return;
            case DroneState.TAKING_OFF:
                TakeOff();
                break;
            case DroneState.FLYING:
                break;
            case DroneState.LANDING:
                Land();
                break;
            case DroneState.DESACTIVATING_ROTORS:
                ChangeStateIfAllRotorsAreFullyDesarmed();
                return;
            default:
                break;
        }


        MovementUpDown();
        MovementForward();
        MovementSideways();
        Rotation();
        ClampingVelocity();
        rb.AddRelativeForce(Vector3.up * upForce * rb.mass);
        rb.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, currentYrotation, tiltAmountSideways)
        );
    }


    #region Movement & rotation
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
            IsGrounded = false;
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

    private void MovementForward()
    {
        int orientation = Pitch == Pitch.Forward ? 1 : Pitch == Pitch.Backward ? -1 : 0;
        rb.AddRelativeForce(Vector3.forward * orientation * forwardSpeed);
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, forwardAngle * orientation, ref tiltVelocityForward, 0.5f);
    }

    private void Rotation()
    {
        int orientation = Yaw == Yaw.Hours ? 1 : Yaw == Yaw.AntiHours ? -1 : 0;
        wantedYrotation += orientation * rotationByKeys;
        currentYrotation = Mathf.SmoothDamp(currentYrotation, wantedYrotation, ref rotationVelocity, 0.25f);
    }

    private void ClampingVelocity()
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

    private void MovementSideways()
    {
        int orientation = Roll == Roll.Right ? 1 : Roll == Roll.Left ? -1 : 0;
        rb.AddRelativeForce(Vector3.right * orientation * sidewaysdSpeed);
        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -1 * sidewaysAngle * orientation, ref tiltVelocitySideways, 0.3f);
    }

    #endregion


    #region State control

    public void Arm()
    {
        state = DroneState.ACTIVATING_ROTORS;
        foreach (Rotor rotor in rotors)
            rotor.Arm();
    }

    public void ChangeStateIfAllRotorsAreFullyArmed()
    {
        foreach (Rotor rotor in rotors)
            if (rotor.state == Rotor.RotorState.READY)
                return;

        state = DroneState.WAITING_FLY_INSTRUCTIONS_ON_GROUND;
    }

    public void TakeOff()
    {
        state = DroneState.TAKING_OFF;
        Altitude = Altitude.Up;
        StartCoroutine(ChangeStateAfterTakingfOf());
    }

    IEnumerator ChangeStateAfterTakingfOf()
    {
        yield return new WaitForSeconds(takeOffTime);
        state = DroneState.FLYING;
    }

    public void Land()
    {
        state = DroneState.LANDING;
        Altitude = Altitude.Down;
        if (IsGrounded)
            state = DroneState.WAITING_FLY_INSTRUCTIONS_ON_GROUND;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
            IsGrounded = true;
    }

    public void Desarm()
    {
        state = DroneState.DESACTIVATING_ROTORS;
        foreach (Rotor rotor in rotors)
            rotor.Desarm();
    }

    public void ChangeStateIfAllRotorsAreFullyDesarmed()
    {
        foreach (Rotor rotor in rotors)
            if (rotor.state == Rotor.RotorState.OFF)
                return;

        state = DroneState.OFF;
    }

    #endregion
}