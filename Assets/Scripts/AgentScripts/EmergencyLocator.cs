using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmergencyLocator : MonoBehaviour
{
    [SerializeField]
    private float updateTime = 0.1f;
    private float currentTime = 0f;

    [SerializeField]
    private float agentSightRadius = 10f;
    [SerializeField]
    private float agentSightAngle = 65;

    private LayerMask exitLayerMask;

    private List<GameObject> knownExitPoints;

    private GameObject parentExitPoint;

    private bool isEmergency;

    private Vector3 LeftBound;
    private Vector3 RightBound;

    private GoalManager goalManager;

    private void Start()
    {
        exitLayerMask = LayerMask.GetMask("Exit");
        PlaneScript planeScript = (PlaneScript)GameObject.FindObjectOfType(typeof(PlaneScript));
        goalManager = GoalManager.GetInstance();

        knownExitPoints = new List<GameObject>();

        isEmergency = false;
        planeScript.GetBounds(out LeftBound, out RightBound);

    }

    private void Update()
    {

        currentTime += Time.deltaTime;

        if (currentTime > updateTime)
        {
            currentTime = 0;
            CheckLineOfSight();
        }

        Debug.DrawRay(transform.position, transform.forward);
    }

    void CheckLineOfSight()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, agentSightRadius, layerMask: exitLayerMask);

        RaycastHit hit;
        Vector3 rayDirection;

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            Transform targetTransform = hitColliders[i].gameObject.transform;

            rayDirection = targetTransform.position - transform.position;

            RaycastHit tempHit;

            //Debug.Log(Vector3.Angle(transform.forward, rayDirection));

            Physics.Linecast(this.transform.position, targetTransform.position, out tempHit);

            if (tempHit.transform.gameObject.name == targetTransform.name && rayDirection.magnitude < agentSightRadius)
            {

                    if (Vector3.Angle(transform.forward, rayDirection) < agentSightAngle)
                    {
                        //Debug.DrawRay(this.transform.position, rayDirection);
                        //Debug.Log("Angle" + Vector3.Angle(transform.forward, rayDirection) + " max sight " + agentSightAngle);

                        if (!knownExitPoints.Contains(hitColliders[i].gameObject))
                        {
                            knownExitPoints.Add(hitColliders[i].gameObject);
                            if (isEmergency && knownExitPoints.Count == 1)
                            {
                                goalManager.UpdateEmergency(gameObject);
                            }
                        }
                    }
            }
        }
    }

    public void GetExit(out List<Vector3> exitPosition, out List<GameObject> interactionObject)
    {
        if (!isEmergency)
        {
            isEmergency = true;
        }

        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(gameObject.transform.position, out navMeshHit, .3f, NavMesh.AllAreas)) 
        {
            if(navMeshHit.mask == 1)
            {
                Destroy(gameObject);
            }
        }

        Vector3 currentExitPoint = this.transform.position;
        GameObject exitObject = null;

        if (knownExitPoints.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            float currentDistance;
            currentExitPoint = knownExitPoints[0].transform.position;
            exitObject = knownExitPoints[0].gameObject;

            for (int i = 0; i < knownExitPoints.Count; ++i)
            {
                currentDistance = (knownExitPoints[i].transform.position - transform.position).magnitude;

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    currentExitPoint = knownExitPoints[i].transform.position;
                    exitObject = knownExitPoints[i].gameObject;
                }
            }
        }
        else
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, agentSightRadius);
            KinematicEntity agentKinematic;

            Vector3 overallDirection = Vector3.zero;

            List<GameObject> nearAgents = new List<GameObject>();

            for (int i = 0; i < hitColliders.Length; ++i)
            {

                if (hitColliders[i].tag == "Agent")
                {
                    nearAgents.Add(hitColliders[i].gameObject);
                }

            }

            for (int i = 0; i < nearAgents.Count; ++i)
            {
                agentKinematic = nearAgents[i].GetComponent<KinematicEntity>();
                overallDirection += agentKinematic.GetVelocity();
            }

            float agentBehaviour = Random.Range(0, 1f);

            if (nearAgents.Count > 0 && agentBehaviour < 0.1f)
            {
                currentExitPoint = overallDirection / nearAgents.Count;
            }
            else
            {
                currentExitPoint.x = Random.Range(LeftBound.x, RightBound.x);
                currentExitPoint.z = Random.Range(LeftBound.z, RightBound.z);
            }
        }


        /*
        Debug.Log("Left bound x " + LeftBound.x);
        Debug.Log("Right bound x " + RightBound.x);
        Debug.Log("Random number" + Random.Range(LeftBound.x, RightBound.x));
        Debug.Log("Exit proposed" + currentExitPoint);
        */


        NavMeshHit hit;
        int navMeshMask = NavMesh.GetAreaFromName("Interior");
        NavMesh.SamplePosition(currentExitPoint, out hit, Mathf.Infinity, navMeshMask);

        currentExitPoint = hit.position;



        if (exitObject != null)
        {
            exitPosition = new List<Vector3>() { exitObject.gameObject.transform.position};
            interactionObject = new List<GameObject> { exitObject };

            parentExitPoint = exitObject.GetComponent<ExitPoint>().GetParent();

            while (parentExitPoint != null)
            {
                interactionObject.Add(parentExitPoint);
                exitPosition.Add(parentExitPoint.transform.position);

                parentExitPoint = parentExitPoint.GetComponent<ExitPoint>().GetParent();
            }
        }
        else
        {
            exitPosition = new List<Vector3> { currentExitPoint };
            interactionObject = new List<GameObject>();

        }

    }


    void OnDrawGizmosSelected()
    {
        Color colliderColor = Color.green;
        colliderColor.a = 0.25f;
        Gizmos.color = colliderColor;

        Gizmos.DrawSphere(transform.position, agentSightRadius);
    }



}
