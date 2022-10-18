using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex;

    // Use this for initialization
    private void Start()
    {
        currentCameraIndex = 0;

        //Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        //If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
            //Debug.Log("Camera with name: " + cameras[0].GetComponent<Camera>().name + ", is now enabled");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //If the c button is pressed, switch to the next camera
        //Set the camera at the current index to inactive, and set the next one in the array to active
        //When we reach the end of the camera array, move back to the beginning or the array.
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log("C button has been pressed. Switching to the next camera");
            cameras[currentCameraIndex].gameObject.SetActive(false);
            Debug.Log($"Length = {cameras.Length}, currentCameraIndex= {currentCameraIndex}, (currentCameraIndex + 1) = {(currentCameraIndex + 1)},  Modulo = {(currentCameraIndex + 1) % cameras.Length}");
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            cameras[currentCameraIndex].gameObject.SetActive(true);
            //Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
        }
    }
}