using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    FORWARD,
    BACKWARD,
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
}

public class AutonomousController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 TargetPosition;
    private Direction direction = Direction.NONE;

    [Header("Speed")]
    [SerializeField] private float speed;

    [SerializeField] private float AltitudeCoef;

    [Header("Sensor")]
    [SerializeField] private float DistanceFromTheWall;

    [SerializeField] private DistanceSensor ForwardSensor;

    [Header("Angle")]
    [SerializeField] private float forwardAngle = 20;

    private float tiltAmountForward = 0;
    private float tiltVelocityForward;

    [SerializeField] private float Yrotation = 20;
    [SerializeField] private float sidewaysAngle = 20;
    private float tiltAmountSideways = 0;
    private float tiltVelocitySideways;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        AltitudeToReach = 1;
    }

    private void FixedUpdate()
    {
        GetDirection();
        Move();
        Rotation();
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, step);

        rb.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, rb.rotation.y, tiltAmountSideways)
        );
    }

    private int orientationSideways = 1;
    private float AltitudeToReach;

    private bool NeedToRefocusOnTheWall()
    {
        return ForwardSensor.NearDistance(DistanceFromTheWall);
    }

    private void GetDirection()
    {
        //If Moving forward Axis do nothing
        if (direction == Direction.FORWARD || direction == Direction.BACKWARD)
        {
            bool reachForward = transform.position.z.CompareToWithApproximation(TargetPosition.z, 0.05f) == 0;
            if (!reachForward)
            {
                return;
            }
            direction = Direction.NONE;
        }

        bool needToRefocusOnTheWall = NeedToRefocusOnTheWall();
        if (needToRefocusOnTheWall)
        {
            float distance = ForwardSensor.Distance - DistanceFromTheWall;
            direction = distance > 0 ? Direction.FORWARD : Direction.BACKWARD;

            TargetPosition = transform.position + Vector3.forward * distance;
            Debug.Log("GoBackward");
        }
        else if (transform.position.y.CompareToWithApproximation(AltitudeToReach, 0.05f) != 0)
        {
            direction = Direction.UP;
            Debug.Log("GoUp");
        }
        else if (!ForwardSensor.HitSomething() && direction != Direction.UP)
        {
            orientationSideways *= -1;
            AltitudeToReach = transform.position.y + AltitudeCoef;
            direction = Direction.UP;
            Debug.Log("ChangeDirection");
        }
        else
        {
            direction = orientationSideways > 0 ? Direction.RIGHT : Direction.LEFT;
            Debug.Log("GoSideways");
        }
    }

    private void Move()
    {
        switch (direction)
        {
            case Direction.UP:
                TargetPosition = transform.position + Vector3.up;
                break;

            case Direction.DOWN:
                TargetPosition = transform.position + Vector3.down;
                break;

            case Direction.LEFT:
                TargetPosition = transform.position + Vector3.left;
                break;

            case Direction.RIGHT:
                TargetPosition = transform.position + Vector3.right;
                break;

            default:
                TargetPosition = TargetPosition;
                break;
        }
    }

    private void Rotation()
    {
        float appro = 0.05f;
        int orientationForward = TargetPosition.z.CompareToWithApproximation(transform.position.z, appro);
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, forwardAngle * orientationForward, ref tiltVelocityForward, 0.5f);

        int orientationright = transform.position.x.CompareToWithApproximation(TargetPosition.x, appro);
        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, sidewaysAngle * orientationright, ref tiltVelocitySideways, 0.5f);
    }
}