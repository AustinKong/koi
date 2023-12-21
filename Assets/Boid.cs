using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [HideInInspector]
    public Vector2 velocity;

    //unique values
    public float minSpeed;
    public float maxSpeed;

    private void Update()
    {
        List<Boid> neighbours = GetNeighbours(BoidManager.boidList);
        Cohesion(neighbours);
        Avoidance(neighbours);
        Alignment(neighbours);
        ObstacleAvoidance();

        if (velocity.magnitude < minSpeed) velocity = velocity.normalized * minSpeed;
        transform.position += (Vector3)velocity * Time.deltaTime;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity), 135f * Time.deltaTime);
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Cohesion(List<Boid> neighbours)
    {
        Vector2 relativeAveragePosition = Vector2.zero;
        int count = 0;
        foreach(Boid target in neighbours)
        {
            if(Vector2.Distance(target.transform.position, transform.position) < BoidManager.cohesionRadius)
            {
                count++;
                relativeAveragePosition += (Vector2)target.transform.position;
            }
        }
        if (count == 0) return;
        relativeAveragePosition /= count;
        relativeAveragePosition -= (Vector2)transform.position;

        velocity += BoidManager.cohesionWeight * Time.deltaTime * Steering(relativeAveragePosition);
    }

    private void Avoidance(List<Boid> neighbours)
    {
        foreach (Boid target in neighbours)
        {
            if (Vector2.Distance(target.transform.position, transform.position) < BoidManager.avoidanceRadius)
            {
                Vector2 delta = target.transform.position - transform.position;
                velocity += BoidManager.avoidanceWeight * Time.deltaTime * Steering(-Vector2.Lerp(delta, Vector2.zero, delta.magnitude / BoidManager.avoidanceRadius));
            }
        }
    }

    private void Alignment(List<Boid> neighbours)
    {
        Vector2 averageVelocity = Vector2.zero;
        int count = 0;
        foreach (Boid target in neighbours)
        {
            if (Vector2.Distance(target.transform.position, transform.position) < BoidManager.alignmentRadius)
            {
                count++;
                averageVelocity += target.velocity;
            }
        }
        if (count == 0) return;
        averageVelocity /= count;

        velocity += BoidManager.alignmentWeight * Time.deltaTime * Steering(averageVelocity);
    }


    void ObstacleAvoidance()
    {
        if (!Physics2D.CircleCast(transform.position, 1f, velocity.normalized, BoidManager.raycastDistance)) return;
        for (int i = 0; i < (int)(180/BoidManager.raycastIncrement); i++)
        {
            float angle = Mathf.Pow(-1, i) * BoidManager.raycastIncrement * i;
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * velocity.normalized;
            if(!Physics2D.CircleCast(transform.position, 1f, direction, BoidManager.raycastDistance))
            {
                velocity += BoidManager.obstacleAvoidanceWeight * Time.deltaTime * Steering(direction);
            }
        }
    }

    public IEnumerator Startled(float duration, Vector2 direction, float weight)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return 0;
            velocity += Time.deltaTime * weight * Steering(direction);
        }
    }

    private Vector2 Steering(Vector2 vector)
    {
        Vector2 v = vector.normalized * maxSpeed - velocity;
        return Vector2.ClampMagnitude(v, BoidManager.maxSteerForce);
    }

    private List<Boid> GetNeighbours(List<Boid> boidList)
    {
        List<Boid> newBoidList = new List<Boid>();
        
        foreach(Boid target in boidList)
        {
            Vector2 delta = target.transform.position - transform.position;
            if (Vector2.Distance(target.transform.position, transform.position) < BoidManager.perceptionRadius && Vector2.Angle(velocity, delta) < BoidManager.perceptionHalfAngle)
            {
                newBoidList.Add(target);
            }
        }
        newBoidList.Remove(this);

        return newBoidList;
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, BoidManager.perceptionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, BoidManager.cohesionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BoidManager.avoidanceRadius);
        
    }
    */
}
