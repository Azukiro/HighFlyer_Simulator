using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right,
    ForwardLeft,
    ForwardRight,
    BackwardLeft,
    BackwardRight,
    None
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

public class DroneController : MonoBehaviour
{
    private Rigidbody rb;
    private float up_down_axis, forward_backward_axis, right_left_axis;
    private float forward_backward_angle = 0, right_left_angle = 0;

    [SerializeField]
    private float speed = 1.3f, angle = 25;

    private bool isOnGround = false;

    public Direction direction = Direction.None;
    public Pitch Pitch = Pitch.None;
    public Roll Roll = Roll.None;
    public Yaw Yaw = Yaw.None;
    public Altitude Altitude = Altitude.None;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Controlls();
        transform.localEulerAngles = Vector3.back * right_left_angle + Vector3.right * forward_backward_angle;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(right_left_axis, up_down_axis, forward_backward_axis);
        //rb.AddRelativeForce(right_left_axis, up_down_axis, forward_backward_axis);
    }

    // Update is called once per frame
    private void Controlls()
    {
        if (Altitude == Altitude.Up)
        {
            up_down_axis = 1 * speed;
            isOnGround = false;
        }
        else if (Altitude == Altitude.Down)
        {
            up_down_axis = -1 * speed;
        }
        else
        {
            up_down_axis = 0.01f;
        }

        if (Pitch == Pitch.Forward)
        {
            Debug.Log("Forward");
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, angle, Time.deltaTime);
            forward_backward_axis = speed;
        }
        else if (Pitch == Pitch.Backward)
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, -angle, Time.deltaTime);
            forward_backward_axis = -speed;
        }
        else
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, 0, Time.deltaTime);
            forward_backward_axis = 0;
        }

        if (Roll == Roll.Right)
        {
            right_left_angle = Mathf.Lerp(right_left_angle, angle, Time.deltaTime);
            right_left_axis = speed;
        }
        else if (Roll == Roll.Left)
        {
            right_left_angle = Mathf.Lerp(right_left_angle, -angle, Time.deltaTime);
            right_left_axis = -speed;
        }
        else
        {
            right_left_angle = Mathf.Lerp(right_left_angle, 0, Time.deltaTime);
            right_left_axis = 0;
        }

        if (Pitch == Pitch.Forward && Roll == Roll.Right)
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, angle, Time.deltaTime);
            forward_backward_axis = 0.5f * speed;
            right_left_axis = 0.5f * speed;
        }
        else if (Pitch == Pitch.Forward && Roll == Roll.Left)
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, -angle, Time.deltaTime);
            forward_backward_axis = 0.5f * speed;
            right_left_axis = 0.5f * -speed;
        }
        else if (Pitch == Pitch.Backward && Roll == Roll.Right)
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, -angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, angle, Time.deltaTime);
            forward_backward_axis = 0.5f * -speed;
            right_left_axis = 0.5f * speed;
        }
        else if (Pitch == Pitch.Backward && Roll == Roll.Left)
        {
            forward_backward_angle = Mathf.Lerp(forward_backward_angle, -angle, Time.deltaTime);
            right_left_angle = Mathf.Lerp(right_left_angle, -angle, Time.deltaTime);
            forward_backward_axis = 0.5f * -speed;
            right_left_axis = 0.5f * -speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
        }
    }
}