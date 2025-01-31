using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapComponent : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 targetPosition;

    private bool isActivated = false;

    private void Update()
    {
       // if (isActivated)
      //  {
            MoveTrap();
      //  }
    }

    private void MoveTrap()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = true;
        }
    }
}
