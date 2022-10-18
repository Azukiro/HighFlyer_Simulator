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
    public float angle;
    public Vector3 behindPosition;
    private Vector3 velocityCamera;
    private Transform droneTransform;
    private DronePhysics droneController;

    // Start is called before the first frame update
    private void Awake()
    {
        droneController = GameObject.FindGameObjectWithTag("Player").GetComponent<DronePhysics>();
        droneTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        int orientation = droneController.Pitch == Pitch.Forward ? 1 : droneController.Pitch == Pitch.Backward ? -1 : 0;
        transform.position = Vector3.SmoothDamp(transform.position, droneTransform.transform.TransformPoint(behindPosition), ref velocityCamera, 0.1f);
        transform.rotation = Quaternion.Euler(angle, droneTransform.localEulerAngles.y, 0);
    }
}