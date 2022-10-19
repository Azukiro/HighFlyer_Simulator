/*
** Author : Lucas BILLARD
** Date Create : 13/10/2022
** Description :
*   Class for move the drone in zigzag in front of a wall and stop when the top of the wall is reached
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneMovement : MonoBehaviour
{
    //property rigidbody needed
    private Rigidbody rb;

    public float speed;
    public float rotationSpeed;
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
        horizontal = 1;
        vertical = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        ComputeWallsBorder();
        //Move right using velocity if key is pressed
        if (vertical == 0)
        {
            //rb.velocity = (transform.right * horizontal) * speed * Time.fixedDeltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        //int layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

        //// This would cast rays only against colliders in layer 8.
        //// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

        //RaycastHit hit;
        //// Does the ray intersect any objects excluding the player layer
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //    hitAgain = true;
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //    if (hitAgain)
        //    {
        //        horizontal *= -1;
        //        hitAgain = false;
        //        vertical = 1;
        //        lastY = transform.position.y;
        //    }
        //}
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
                lastY = transform.position.y;
            }

            if (transform.position.x < rightBorderX && transform.position.x > leftBorderX)
            {
                speed = 0;
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
            Debug.Log(vertical);
            Vector3 newVelocity = (transform.right * horizontal) * speed * Time.fixedDeltaTime;
            if (distance > distanceOptimale + 0.2 || distance < distanceOptimale - 0.2)
            {
                int front = distance > distanceOptimale ? 1 : -1;
                newVelocity += (transform.forward * front) * speed * Time.fixedDeltaTime;
            }
            rb.velocity = newVelocity;
        }
        else
        {
            rb.velocity = (transform.up * vertical) * speed * Time.fixedDeltaTime;
            if (transform.position.y - lastY > UpPixel)
            {
                vertical = 0;
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