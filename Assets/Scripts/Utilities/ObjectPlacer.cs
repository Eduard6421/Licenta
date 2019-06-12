using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{

    [SerializeField]
    private GameObject WallObstacle = null;


    private GameObject CurrentObjectBuilding;

    [SerializeField]
    private KeyCode newWallHotKey = KeyCode.B;


    private float mouseWheelRotation;

    private Vector3 scaleVector = new Vector3(0.1f, 0, 0);
    private Vector3 defaultScaleVector = new Vector3(1, 1, 1);


    private void Update()
    {
        HandeBuildingJob();

        if(CurrentObjectBuilding != null)
        {
            MoveObjectToMousePosition();
            RotateWithMouseWheel();
            ScaleWithPlus();
            ReleaseIfClicked(); 
        }
    }

    private void ScaleWithPlus()
    {

        if (Input.GetKey(KeyCode.Y))
        {
            CurrentObjectBuilding.GetComponent<Transform>().localScale += scaleVector;
        }
        if(Input.GetKey(KeyCode.U))
        {
            Transform objectTransform = CurrentObjectBuilding.GetComponent<Transform>();
            objectTransform.localScale -= scaleVector;

            if (objectTransform.localScale.x < 1)
            {
                objectTransform.localScale = defaultScaleVector;
            }
        }

    }

    private void MoveObjectToMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        LayerMask mask = LayerMask.GetMask("Floor");

        if(Physics.Raycast(ray , out hitInfo, Mathf.Infinity, mask))
        {
            CurrentObjectBuilding.transform.position = hitInfo.point;
            CurrentObjectBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void HandeBuildingJob()
    {

        if(Input.GetKeyDown(newWallHotKey))
        {
            CurrentObjectBuilding = Instantiate(WallObstacle);
            CurrentObjectBuilding.GetComponent<Renderer>().material.color = Color.green;
        }
    }


    private void RotateWithMouseWheel()
    {

        mouseWheelRotation += Input.mouseScrollDelta.y;
        CurrentObjectBuilding.transform.Rotate(Vector3.up, mouseWheelRotation * 5f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CurrentObjectBuilding = null;
        }
    }




}
