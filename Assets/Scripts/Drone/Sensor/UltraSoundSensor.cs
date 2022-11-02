using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpaceGap
{
    BETWEEN,
    EVENLY,
    ARROUND
}
public class UltraSoundSensor : DistanceSensor
{
    private Collider bx;
    [SerializeField] private int nb_raycast;
    [SerializeField] private SpaceGap space;

    private void Awake()
    {
        bx = GetComponentInParent<Collider>();
    }

    // Start is called before the first frame update
    protected override float GetSensorData()
    {
        float width = bx.bounds.size.x;
        float gap;
        switch (space)
        {
            case SpaceGap.ARROUND:
                gap = width / (nb_raycast );
                break;
            case SpaceGap.EVENLY:
                gap = width / (nb_raycast + 1);
                break;
            case SpaceGap.BETWEEN:
            default:
                gap = width / (nb_raycast-1);
                break;
        }
        float initialPosition = transform.position.x - width / 2;

        float distance = float.MaxValue;
        RaycastHit hit;
        bool hitting;
        for (int i = 0; i < nb_raycast; i++)
        {

            float newZ = initialPosition;

            switch (space)
            {
                case SpaceGap.ARROUND:
                    newZ += gap / 2 + i * gap;
                    break;
                case SpaceGap.EVENLY:
                    newZ += gap + i * gap;
                    break;
                case SpaceGap.BETWEEN:
                default:
                    newZ += i * gap;
                    break;

            }
            Vector3 newPosition = new Vector3(newZ, transform.position.y, transform.position.z);
            hitting = Physics.Raycast(
                newPosition,
                transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity);

            if (hitting && CollisionObjectIsValid(hit.transform.tag))
            {
                Debug.DrawRay(newPosition, transform.TransformDirection(Vector3.forward) * hit.distance, HitColor);

                if (hit.distance < distance)
                    distance = hit.distance;
            }
            else
            {
                Debug.DrawRay(newPosition, transform.TransformDirection(Vector3.forward) * 1000, NotHitColor);
            }
        }

        return distance;
    }
}