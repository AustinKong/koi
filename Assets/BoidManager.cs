using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public const float perceptionHalfAngle = 130f;
    public const float perceptionRadius = 4f;
    public const float cohesionRadius = 4f;
    public const float avoidanceRadius = 2f;
    public const float alignmentRadius = 3f;
    public const float raycastDistance = 1.5f;
    public const float raycastIncrement = 10f;

    public const float cohesionWeight = 1.5f;
    public const float avoidanceWeight = 1.8f;
    public const float alignmentWeight = 0.9f;
    public const float obstacleAvoidanceWeight = 10f;

    public const float minSpeed = 2f;
    public const float maxSpeed = 3f;
    public const float maxSteerForce = 1f;

    public static List<Boid> boidList = new List<Boid>();

    private void Start()
    {
        boidList.AddRange(FindObjectsOfType<Boid>());
        foreach(Boid boid in boidList)
        {
            boid.velocity = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            boid.maxSpeed = maxSpeed + Random.Range(0, 1f);
            boid.minSpeed = minSpeed - Random.Range(0, 1f);
            boid.GetComponentInChildren<BoidBody>().timeOffset = Random.Range(-2f, 2f);


            Color col = new Color(1,1,1);//new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            LineRenderer lr = boid.GetComponentInChildren<LineRenderer>();
            lr.startColor = col; //Color.HSVToRGB(0.94f, 0.25f / colorValue, 1);
            lr.endColor = col;//Color.HSVToRGB(0.94f, 0.25f / colorValue, 1);
            lr.startWidth = Random.Range(0.5f, 1.1f);
            lr.endWidth = Random.Range(0.5f, 1.1f);
        }
    }
}
