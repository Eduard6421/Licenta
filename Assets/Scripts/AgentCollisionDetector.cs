using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCollisionDetector : MonoBehaviour
{

    private KinematicEntity agentKinematic;
    private KinematicEntity otherAgentKinematic;

    private Vector3 agentVelocity;
    private Vector3 otherAgentVelocity;

    private void Awake()
    {
        agentKinematic = this.GetComponent<KinematicEntity>();
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.tag == "Agent")
        {
            otherAgentKinematic = collision.collider.gameObject.GetComponent<KinematicEntity>();

            agentVelocity = agentKinematic.GetVelocity();
            otherAgentVelocity = otherAgentKinematic.GetVelocity();

            if(agentVelocity.magnitude < otherAgentVelocity.magnitude && otherAgentVelocity.magnitude > 2.5)
            {
                agentKinematic.CapSpeed();
                Debug.Log("Collision :(");
            }

        }
    }

}
