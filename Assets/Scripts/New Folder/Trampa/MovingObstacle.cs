using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; // Dirección del movimiento
    public float speed = 2f; // Velocidad del movimiento
    public float distance = 5f; // Distancia máxima de movimiento

    private Vector3 startPosition;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        startPosition = transform.position;
        journeyLength = Vector3.Distance(startPosition, startPosition + direction * distance);
        startTime = Time.time;
    }

    void Update()
    {
        float distanceTravelled = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceTravelled / journeyLength;
        transform.position = startPosition + direction * Mathf.PingPong(fractionOfJourney, 1) * distance;
    }
}

