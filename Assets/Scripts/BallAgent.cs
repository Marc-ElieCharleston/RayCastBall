using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;



public class BallAgent : Agent
{
    public GameObject target;

    public float speed = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float rayDistance = 10f;
        float[] rayAngles = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f };

        foreach (float angle in rayAngles)
        {
            Vector3 rayDirection = Quaternion.Euler(0f, angle, 0f) * transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    // Obstacle (mur) détecté, ajoute la distance au mur en tant qu'observation
                    sensor.AddObservation(hit.distance / rayDistance); // Distance normalisée entre 0 et 1
                }
                else if (hit.collider.CompareTag("Target"))
                {
                    // Cible détectée, ajoute la distance à la cible en tant qu'observation
                    sensor.AddObservation(hit.distance / rayDistance); // Distance normalisée entre 0 et 1
                }
                else
                {
                    sensor.AddObservation(0f); // Aucun objet détecté
                }
            }
            else
            {
                sensor.AddObservation(0f); // Aucun objet détecté
            }
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float verticalInput = actions.ContinuousActions[0];
        float horizontalInput = actions.ContinuousActions[1];

        transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var agentContinousAction = actionsOut.ContinuousActions;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        agentContinousAction[0] = verticalInput;
        agentContinousAction[1] = horizontalInput;
    }
}
