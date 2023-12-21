using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Camera mainCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickedPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Disturbance(clickedPosition, 4f);
        }
    }

    private void Disturbance(Vector2 position, float radius)
    {
        foreach(Boid target in BoidManager.boidList)
        {
            if(Vector2.Distance(position, target.transform.position) < radius)
            {
                float clampedForce = Mathf.Clamp(radius / Vector2.Distance(position, target.transform.position), 12f, 17f);
                target.StartCoroutine(target.Startled(1f, ((Vector2)target.transform.position - position).normalized, clampedForce));
            }
        }
    }
}
