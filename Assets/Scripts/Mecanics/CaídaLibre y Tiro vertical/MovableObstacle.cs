using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacle : MonoBehaviour
{
    public Vector3 moveDistance = new Vector3(5f, 0f, 0f);
    public float moveSpeed = 2f;
    public float waitTime = 1f; // Tiempo de espera antes de regresar

    private Vector3 startPosition;
    private bool movingForward = true;
    private bool isWaiting = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!isWaiting)
        {
            float step = moveSpeed * Time.deltaTime;
            if (movingForward)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition + moveDistance, step);
                if (Vector3.Distance(transform.position, startPosition + moveDistance) < 0.001f)
                {
                    movingForward = false;
                    StartCoroutine(WaitBeforeMovingBack());
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
                if (Vector3.Distance(transform.position, startPosition) < 0.001f)
                {
                    movingForward = true;
                    StartCoroutine(WaitBeforeMovingBack());
                }
            }
        }
    }

    IEnumerator WaitBeforeMovingBack()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
