using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightCalculator : BaseCalculator
{
    public GameObject midArrow;
    public Arrow midArrowComponent;
    private bool isReturning = false;
    private bool isInRightPath = false;

    private bool middleArrow = false;

    public bool IsReturning { get { return isReturning; } set { isReturning = value; } }

    public override void OnArrowPassed(Arrow arrow)
    {
        if (!isCalculating && arrow.CompareTag(nameof(startArrow)))
        {
            StartCalculations();
        }
        else if (isCalculating)
        {
            HandleArrowPass(arrow);
        }
    }

    private void StartCalculations()
    {
        timeElapsed = 0f;
        totalDistance = 0f;
        netDisplacement = 0f;
        isCalculating = true;
        InitializePositions();
    }

    private void HandleArrowPass(Arrow arrow)
    {
        if (arrow.CompareTag(nameof(midArrow)) && !isReturning)
        {
            HandleMidArrowPass(arrow);
          //  middleArrow = true;

        }
        else if (arrow.CompareTag(nameof(endArrow)) && isInRightPath)
        {
            HandleEndArrowPass(arrow);
        }
    }

    private void HandleMidArrowPass(Arrow arrow)
    {
        arrow.ChangeColor(Color.cyan);
        endArrow.ResetColor();

    }

    private void HandleEndArrowPass(Arrow arrow)
    {
        arrow.ChangeColor(Color.cyan);

        if (!isReturning)
        {
            StartReturn(arrow);
        }
        else
        {
            EndCalculations(arrow);
            
        }
    }

    private void StartReturn(Arrow arrow)
    {
        isReturning = true;
        middleArrow = true;
        arrow.SetEndArrowPassed(true);
        arrow.RotateArrow();
        midArrow.GetComponent<Arrow>().ResetColor();
    }

    private void EndCalculations(Arrow arrow)
    {
        Debug.Log("medio");
        isCalculating = false;
        StartCoroutine(ResetColors(arrow));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightPath"))
        {
            isInRightPath = true;
        }
        else if (other.CompareTag("Reset"))
        {
            isCalculating = false;
            ResetCalculations();
            endArrow.ResetColor();
            midArrowComponent.ResetColor();
            startArrow.ResetColor();
            isReturning = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightPath"))
        {
            isInRightPath = false;
        }
    }

    private IEnumerator ResetColors(Arrow arrow)
    {
        yield return new WaitForSeconds(3f);
        startArrow.ResetColor();
        midArrowComponent.ResetColor();
        endArrow.ResetColor(); // Cambiado de endArrowComponent a endArrow
        arrow.ResetArrow();
    }

    protected override void UpdateConditionChecker(float speed, float rapidity)
    {
        if (middleArrow)
        {
            conditionChecker.UpdateRightVelocityStatus(speed >= requiredVelocity);
            conditionChecker.UpdateRightSpeedStatus(rapidity >= requiredSpeed);
        }
    }
}
