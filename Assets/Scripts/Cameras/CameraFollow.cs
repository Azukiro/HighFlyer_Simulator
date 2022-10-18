/*
** Author : Lucas BILLARD
** Source : Mario Haberle - Drone Tutoria Unity3d C# - https://www.youtube.com/playlist?list=PLPAgqhxd1Ib1YYqYnZioGyrSUzOwead17
** Date Create : 18/10/2022
** Description :
*   Class for following the Player to a specific behin position and angle
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public float smooth = 1f;
    public Vector3 behindPosition;

    private Vector3 velocityCamera;
    private Transform droneTransform;

    // Start is called before the first frame update
    private void Awake()
    {
        droneTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    { 
        transform.position = Vector3.SmoothDamp(transform.position, droneTransform.transform.TransformPoint(behindPosition), ref velocityCamera, smooth * Time.fixedDeltaTime);
        transform.LookAt(droneTransform);
    }
}