using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour
{

    float mainSpeed = 50.0f; 
    float shiftAdd = 250.0f; 
    float maxShift = 1000.0f; 
    float camSensibility = 3f; 
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun = 1.0f;
    private Vector3[] cameraVelocity = new Vector3[] { new Vector3(0, 0, 1),
                new Vector3(0, 0, -1) , new Vector3(-1,0,0), new Vector3(1,0,0)};

    private bool moveMode = false;



    private Vector3 BoundPosition(Vector3 currentPosition)
    {

        Vector3 newPosition = new Vector3();

        if (currentPosition.y < 1)
        {
            newPosition.y = 1;
        }

        if (Mathf.Abs(currentPosition.x) > 1000)
        {
            currentPosition.x = Mathf.Sign(currentPosition.x) * 1000;
        }

        if (Mathf.Abs(currentPosition.z) > 1000)
        {
            currentPosition.z = Mathf.Sign(currentPosition.z) * 1000;
        }

        return newPosition;
    }

    private Vector3 GetMovementInput()
    {
        Vector3 camVelocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            camVelocity += cameraVelocity[0];
        }
        if (Input.GetKey(KeyCode.S))
        {
            camVelocity += cameraVelocity[1];
        }
        if (Input.GetKey(KeyCode.A))
        {
            camVelocity += cameraVelocity[2];
        }
        if (Input.GetKey(KeyCode.D))
        {
            camVelocity += cameraVelocity[3];
        }

        return camVelocity;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            moveMode = !moveMode;
            lastMouse = Input.mousePosition;

            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }


        if (moveMode)
        {

            Vector3 newAngle = new Vector3(transform.eulerAngles.x - camSensibility* Input.GetAxis("Mouse Y"), transform.eulerAngles.y + camSensibility* Input.GetAxis("Mouse X"), 0);

            transform.eulerAngles = newAngle;

            Vector3 inputVector = GetMovementInput();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                inputVector = inputVector * totalRun * shiftAdd;
                inputVector.x = Mathf.Clamp(inputVector.x, -maxShift, maxShift);
                inputVector.y = Mathf.Clamp(inputVector.y, -maxShift, maxShift);
                inputVector.z = Mathf.Clamp(inputVector.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                inputVector = inputVector * mainSpeed;
            }

            inputVector = inputVector * Time.deltaTime;
            Vector3 newPosition = transform.position;


            if (Input.GetKey(KeyCode.Space))
            {
                transform.Translate(inputVector);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(inputVector);
            }

            transform.Translate(BoundPosition(this.transform.position));

        }

    }

}