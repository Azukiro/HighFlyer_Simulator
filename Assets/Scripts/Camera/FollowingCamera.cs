using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField]
    public GameObject target;

    public int horizontalDistance = 10;
    public int verticalDistance   = 2;
    public float smooth           = 1f;

    private void Update()
    {
        float x = target.transform.position.x - target.transform.forward.x * horizontalDistance;
        float y = target.transform.position.y + verticalDistance;
        float z = target.transform.position.z - target.transform.forward.z * horizontalDistance;
        Vector3 posToReach = new Vector3(x, y, z);
        transform.position = Vector3.Lerp(transform.position, posToReach, smooth * Time.deltaTime);
        transform.LookAt(target.transform);
    }
}
