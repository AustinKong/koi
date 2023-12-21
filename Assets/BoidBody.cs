using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBody : MonoBehaviour
{
    public int vertexCount = 5;
    public float vertexDistance = 0.2f;
    public float smoothSpeed = 0.005f;
    public float wiggleFrequency = 1f;
    public float wiggleMagnitude = 1f;

    public Transform wiggleDirection;
    public Transform targetDirection;
    public LineRenderer lineRenderer;

    private Vector3[] vertexPositions;
    private Vector3[] vertexVelocity;

    [HideInInspector]
    public float timeOffset = 0f; //wiggle sin wave offset

    private void Start()
    {
        lineRenderer.positionCount = vertexCount;
        vertexPositions = new Vector3[vertexCount];
        vertexVelocity = new Vector3[vertexCount];
    }

    private void Update()
    {
        wiggleDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleFrequency + timeOffset) * wiggleMagnitude);
        vertexPositions[0] = targetDirection.position;
        for(int i = 1; i < vertexPositions.Length; i++)
        {
            Vector3 targetPosition = vertexPositions[i - 1] + (vertexPositions[i] - vertexPositions[i - 1]).normalized * vertexDistance;
            vertexPositions[i] = Vector3.SmoothDamp(vertexPositions[i], targetPosition, ref vertexVelocity[i], smoothSpeed);
        }
        lineRenderer.SetPositions(vertexPositions);
    }
}
