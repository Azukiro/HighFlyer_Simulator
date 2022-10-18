using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 150f;

    private float speed = 0f;

    [SerializeField]
    private float approximation = 0.2f;

    [SerializeField]
    private float accelerationCoeff = 1f;

    public enum RotorState
    {
        TAKE_OFF,
        FLY,
        LAND,
        OFF
    }

    public RotorState state = RotorState.TAKE_OFF;

    private void FixedUpdate()
    {
        if (state == RotorState.TAKE_OFF)
        {
            speed = Mathf.Lerp(speed, maxSpeed, accelerationCoeff * Time.fixedDeltaTime);
            transform.Rotate(0, 0, speed);

            if (Mathf.Abs(maxSpeed - speed) < approximation)
            {
                speed = maxSpeed;
                state = RotorState.FLY;
            }
        }

        if (state == RotorState.FLY)
        {
            transform.Rotate(0, 0, maxSpeed);
        }

        if (state == RotorState.LAND)
        {
            speed = Mathf.Lerp(speed, 0, accelerationCoeff * Time.fixedDeltaTime);
            transform.Rotate(0, 0, speed);

            if (Mathf.Abs(speed - 0) < approximation)
            {
                speed = 0;
                state = RotorState.OFF;
            }
        }
    }

    public void TakeOff()
    {
        if (state == RotorState.OFF)
            state = RotorState.TAKE_OFF;
    }

    public void Land()
    {
        if (state == RotorState.FLY)
            state = RotorState.LAND;
    }
}