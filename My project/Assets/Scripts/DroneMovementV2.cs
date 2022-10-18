using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneMovementV2 : MonoBehaviour
{
    //property rigidbody needed
    private Rigidbody rb;
    private DroneController controller;
  
    private float vertical;
    private float horizontal;
    private bool hitAgain;
    private float lastY;

    private float rightBorderX = Mathf.Infinity;
    private float leftBorderX = Mathf.Infinity;

    private float UpPixel = 2;
    private float distanceOptimale = 3;

    // Start is called before the first frame update
    private void Start()
    {
        //get the rigidbody component
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<DroneController>();
        controller.direction = Direction.Right;
        horizontal = 1;
        vertical = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        ComputeWallsBorder();
       
    }

    private void ComputeWallsBorder()
    {
        float distance = GetSensorData();
        if (distance == 0)
        {
            if (rightBorderX == Mathf.Infinity)
            {
                rightBorderX = transform.position.x - 0.2f;
            }
            else if (leftBorderX == Mathf.Infinity && transform.position.x < rightBorderX)
            {
                leftBorderX = transform.position.x + 0.2f;
            }
            if (hitAgain)
            {
                horizontal *= -1;
                hitAgain = false;
                vertical = 1;
                controller.goUp = true;
                lastY = transform.position.y;
            }

            if (transform.position.x < rightBorderX && transform.position.x > leftBorderX)
            {
                controller.direction = Direction.None;
            }
        }
        else
        {
            hitAgain = true;
        }

        Move(distance);
    }

    private void Move(float distance)
    {
        if (vertical == 0)
        {
            // Vector3 newVelocity = (transform.right * horizontal) * speed * Time.fixedDeltaTime;
            // // if (distance > distanceOptimale + 0.2 || distance < distanceOptimale - 0.2)
            // // {
            // //     int front = distance > distanceOptimale ? 1 : -1;
            // //     newVelocity += (transform.forward * front) * speed * Time.fixedDeltaTime;
            // // }
            // rb.velocity = newVelocity;
        }
        else
        {
            
            if (transform.position.y - lastY > UpPixel)
            {
                vertical = 0;
                if(controller.direction.Equals(Direction.Left))
                {
                    controller.direction = Direction.Right;
                }else{
                    controller.direction = Direction.Left;
                }
                controller.goUp = null;
            }
        }
    }

    private float GetSensorData()
    {
        int layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        RaycastHit hit;
        bool hitting = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask);

        //Unity Part
        if (hitting)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did NOT Hit");
        }

        return hit.distance;
    }
}