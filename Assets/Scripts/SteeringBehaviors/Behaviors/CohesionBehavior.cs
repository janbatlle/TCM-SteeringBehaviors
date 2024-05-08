using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CohesionBehavior : Steering
{
    [SerializeField]
    private float threshold = 2f;
    [SerializeField]
    public float maxAcceleration;

    private Transform[] targets;

    void Start()
    {
        SteeringBehaviorController[] agents = FindObjectsOfType<SteeringBehaviorController>();
        targets = new Transform[agents.Length - 1];
      
        int count = 0;

        foreach (SteeringBehaviorController agent in agents)
        {
            if (agent.gameObject != gameObject)
            {
                targets[count++] = agent.transform;
            }
        }
    }

    public override SteeringData GetSteering(SteeringBehaviorController steeringController)
    {
        SteeringData steering = new SteeringData();
        Vector2 centerOfMass = Vector2.zero;
        int count = 0;

        foreach (Transform target in targets)
        {
            Vector2 toTarget = target.position - transform.position;
            float distance = toTarget.magnitude;

            if (distance < threshold)
            {
                centerOfMass += (Vector2)target.position;
                count++;
            }
            if (count > 0)
            {
                centerOfMass /= count;
                Vector2 direction = centerOfMass - (Vector2)transform.position;
                steering.linear = direction.normalized * maxAcceleration;
            }
        }
        return steering;
    }
}

