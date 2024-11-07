using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileObstacle : MonoBehaviour
{
    public float rangeMove = 3f;
    public float speedMove = 2f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float desplazamiento = Mathf.PingPong(Time.time * speedMove, rangeMove);
        transform.position = initialPosition + new Vector3(0, 0, desplazamiento);
    }
}
