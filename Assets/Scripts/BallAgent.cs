using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class BallAgent : Agent
{
    public GameObject Target;
    private GameObject targetInstance;
    public GameObject Area;

    public float speed = 2.5f;
    private float areaLim = 2.3f;
    private float areaX;
    private float areaZ;

    private Vector3 agentInitialPos;


    public override void Initialize()
    {
        areaX = Area.transform.position.x;
        areaZ = Area.transform.position.z;

        Vector3 agentInitialPos = new Vector3(areaX, 0, areaZ);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(areaX, 0, areaZ);

        if (targetInstance != null)
        {
            Destroy(targetInstance);
        }
        SpawnTarget();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float verticalInput = actions.ContinuousActions[0];
        float horizontalInput = actions.ContinuousActions[1];

        transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);

        SetReward(-0.05f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var agentContinousAction = actionsOut.ContinuousActions;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        agentContinousAction[0] = verticalInput;
        agentContinousAction[1] = horizontalInput;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Collision target");
            Destroy(collision.gameObject);
            SetReward(10.0f);
            //SpawnTarget();
            EndEpisode();
        }
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    Debug.Log("Collision wall");
        //    SetReward(-1.0f);
        //    
        //}

    }

    private void SpawnTarget()
    {
        float limTargetAreaX = Random.Range(-areaLim, areaLim);
        float limTargetAreaY = Random.Range(-areaLim, areaLim);


        Vector3 spawnPosition = new Vector3(limTargetAreaX + areaX, 0, limTargetAreaY + areaZ);

        targetInstance = Instantiate(Target, spawnPosition, Quaternion.identity);
        
    }
}
