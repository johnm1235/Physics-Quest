using System.Collections;
using System.Collections.Generic;
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
    }

    private void OpenDoor()
    {
        // Aquí podrías activar una animación o cambiar el estado de la puerta
        // door.GetComponent<Animator>().SetTrigger("Open");
        Debug.Log("¡Puerta abierta! Se cumplieron ambas condiciones.");
    }

    public void UpdateLeftVelocityStatus(bool status)
    {
        leftVelocity = status;
        UpdateIndicatorColor(leftVelocityIndicator, status);
        CheckConditions();
    }

    public void UpdateLeftSpeedStatus(bool status)
    {
        leftSpeed = status;
        UpdateIndicatorColor(leftSpeedIndicator, status);
        CheckConditions();
    }

    public void UpdateRightVelocityStatus(bool status)
    {
        rightVelocity = status;
        UpdateIndicatorColor(rightVelocityIndicator, status);
        CheckConditions();
    }

    public void UpdateRightSpeedStatus(bool status)
    {
        rightSpeed = status;
        UpdateIndicatorColor(rightSpeedIndicator, status);
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
