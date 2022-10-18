using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardDrone : MonoBehaviour
{
    private DroneController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<DroneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            controller.direction = Direction.Forward;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            controller.direction = Direction.Backward;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            controller.direction = Direction.Left;
        }
       else  if (Input.GetKeyDown(KeyCode.D))
        {
            controller.direction = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.goUp = true;
        }
       else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.goUp = false;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            controller.goUp = null;
        }else{
            controller.direction = Direction.None;
            controller.goUp = null;
        }

    }
}
