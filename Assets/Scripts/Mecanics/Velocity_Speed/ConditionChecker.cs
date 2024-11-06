using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConditionChecker : MonoBehaviour
{
    public bool leftVelocity = false;
    public bool leftSpeed = false;
    public bool rightVelocity = false;
    public bool rightSpeed = false;

    public GameObject door; // Objeto de la puerta a abrir
    public GameObject leftVelocityIndicator; // Indicador de cumplimiento de velocidad del camino izquierdo
    public GameObject leftSpeedIndicator; // Indicador de cumplimiento de rapidez del camino izquierdo
    public GameObject rightVelocityIndicator; // Indicador de cumplimiento de velocidad del camino derecho
    public GameObject rightSpeedIndicator; // Indicador de cumplimiento de rapidez del camino derecho

    public RightCalculator rightCalculator;
    public LeftCalculator leftCalculator;

    private void Start()
    {
        rightCalculator.enabled = false;
        leftCalculator.enabled = false;
    }

    public void CheckConditions()
    {
        if (leftVelocity && leftSpeed && rightVelocity && rightSpeed)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        door.SetActive(false);
        Debug.Log("¡Puerta abierta! Se cumplieron ambas condiciones.");
    }

    private void CloseDoor()
    {
        door.SetActive(true);
    }

    public void UpdateLeftVelocityStatus(bool status)
    {
        UpdateStatus(ref leftVelocity, leftVelocityIndicator, status);
    }

    public void UpdateLeftSpeedStatus(bool status)
    {
        UpdateStatus(ref leftSpeed, leftSpeedIndicator, status);
    }

    public void UpdateRightVelocityStatus(bool status)
    {
        UpdateStatus(ref rightVelocity, rightVelocityIndicator, status);
    }

    public void UpdateRightSpeedStatus(bool status)
    {
        UpdateStatus(ref rightSpeed, rightSpeedIndicator, status);
    }

    private void UpdateStatus(ref bool condition, GameObject indicator, bool status)
    {
        condition = status;
        UpdateIndicatorColor(indicator, status);
        CheckConditions();
    }

    private void UpdateIndicatorColor(GameObject indicator, bool status)
    {
        indicator.GetComponent<Renderer>().material.color = status ? Color.green : Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftPath"))
        {
            leftCalculator.enabled = true;
            Debug.Log("Trigger entered: " + other.tag);
        }
        else if (other.CompareTag("RightPath"))
        {
            rightCalculator.enabled = true;
            Debug.Log("Trigger entered: " + other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited: " + other.tag);
        if (other.CompareTag("LeftPath"))
        {
            leftCalculator.enabled = false;
        }
        else if (other.CompareTag("RightPath"))
        {
            rightCalculator.enabled = false;
        }
    }
}
