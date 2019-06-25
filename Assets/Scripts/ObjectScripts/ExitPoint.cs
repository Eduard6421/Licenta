using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExitPoint : MonoBehaviour
{
    public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction, color);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 10);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 10);
        Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
        Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
    }

    [SerializeField]
    private GameObject parentExitPoint;



    void OnDrawGizmos()
    {
        Color spawnColor = Color.green;
        Gizmos.color = spawnColor;

        if (parentExitPoint != null)
        {
            Gizmos.DrawLine(transform.position, parentExitPoint.gameObject.transform.position);
        }
    }


    public GameObject GetParent()
    {
        return parentExitPoint;
    }



}
