using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCalculator : BaseCalculator
{
    public override void OnArrowPassed(Arrow arrow)
    {
        Debug.Log("Hola");
        if (!isCalculating && arrow.CompareTag(nameof(startArrow)))
        {
            StartCalculations();
        }
        else if (isCalculating && arrow.CompareTag(nameof(endArrow)))
        {
            EndCalculations();
        }
    }

    private void StartCalculations()
    {
        Debug.Log("StartCalculations called");
        startPosition = transform.position;
        lastPosition = startPosition;
        timeElapsed = 0f;
        totalDistance = 0f;
        netDisplacement = 0f;
        isCalculating = true;
    }

    private void EndCalculations()
    {
        Debug.Log("EndCalculations called");
        isCalculating = false;
        StartCoroutine(ResetColor());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reset"))
        {
            ResetCalculations();
        }
    }

    protected override void UpdateConditionChecker(float speed, float rapidity)
    {
        conditionChecker.UpdateLeftVelocityStatus(speed >= requiredVelocity);
        conditionChecker.UpdateLeftSpeedStatus(rapidity >= requiredSpeed);
    }
}
