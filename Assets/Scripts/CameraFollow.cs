using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float angle;
    public Vector3 behindPosition;
    private Vector3 velocityCamera;
    private Transform droneTransform;
    private MarioDroneController droneController;

    // Start is called before the first frame update
    private void Awake()
    {
        droneController = GameObject.FindGameObjectWithTag("Player").GetComponent<MarioDroneController>();
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