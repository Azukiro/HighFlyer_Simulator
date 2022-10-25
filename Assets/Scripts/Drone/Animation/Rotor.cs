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
        ARMING,
        READY,
        DISARMING,
        OFF
    }

    public RotorState state { get; private set; } = RotorState.OFF;

    private void FixedUpdate()
    {
        if (state == RotorState.ARMING)
        {
            speed = Mathf.Lerp(speed, maxSpeed, accelerationCoeff * Time.fixedDeltaTime);
            transform.Rotate(0, 0, speed);

            if (Mathf.Abs(maxSpeed - speed) < approximation)
            {
                speed = maxSpeed;
                state = RotorState.READY;
            }
        }

        if (state == RotorState.READY)
        {
            transform.Rotate(0, 0, maxSpeed);
        }

        if (state == RotorState.DISARMING)
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

    public void Arm()
    {
        if (state == RotorState.OFF)
            state = RotorState.ARMING;
    }

    public void Desarm()
    {
        if (state == RotorState.READY)
            state = RotorState.DISARMING;
    }
}