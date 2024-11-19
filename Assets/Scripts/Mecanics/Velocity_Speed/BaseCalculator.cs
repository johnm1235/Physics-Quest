using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class BaseCalculator : MonoBehaviour
{
    [Header("Indicators")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rapidityText;
    public TextMeshProUGUI formulaSpeedText;
    public TextMeshProUGUI formulaRapidityText;



    [Header("Arrows")]
    public Arrow startArrow;
    public Arrow endArrow;

    protected Vector3 startPosition;
    protected Vector3 lastPosition;
    protected float timeElapsed = 0f;
    protected bool isCalculating = false;
    protected float totalDistance = 0f;
    protected float netDisplacement = 0f;

    [SerializeField] protected float requiredVelocity;
    [SerializeField] protected float requiredSpeed;

    public ConditionChecker conditionChecker;

    protected virtual void Start()
    {
        InitializePositions();
    }

    protected void InitializePositions()
    {
        startPosition = transform.position;
        lastPosition = startPosition;
    }

    protected virtual void Update()
    {
        if (isCalculating)
        {
            UpdateTime();
            UpdateDistance();
            UpdateCalculations();
        }
    }

    protected void UpdateTime()
    {
        timeElapsed += Time.deltaTime;
    }

    protected void UpdateDistance()
    {
        float distanceTraveled = Vector3.Distance(lastPosition, transform.position);
        totalDistance += distanceTraveled;
        netDisplacement = Vector3.Distance(startPosition, transform.position);
        lastPosition = transform.position;
    }

    protected void UpdateCalculations()
    {
        if (timeElapsed > 0)
        {
            float speed = netDisplacement / timeElapsed;
            float rapidity = totalDistance / timeElapsed;

            UpdateUI(speed, rapidity);
            UpdateConditionChecker(speed, rapidity);
        }
    }

    protected void UpdateUI(float speed, float rapidity)
    {
        int netDisplacementInt = Mathf.FloorToInt(netDisplacement);
        int timeElapsedInt = Mathf.FloorToInt(timeElapsed);
        int rapidityInt = Mathf.FloorToInt(rapidity);
        int totalDistanceInt = Mathf.FloorToInt(totalDistance);

        speedText.text = $"{speed:F2} m/s = {netDisplacement:F2} m / {timeElapsed:F2} s";
        rapidityText.text = $"{rapidity:F2} m/s = {totalDistance:F2} m / {timeElapsed:F2} s";
    }



    protected abstract void UpdateConditionChecker(float speed, float rapidity);


    public void ResetCalculations()
    {
        timeElapsed = 0f;
        totalDistance = 0f;
        netDisplacement = 0f;
        ResetUIText();
    }

    protected void ResetUIText()
    {
        formulaSpeedText.text = "V  = Δx / t";
        formulaRapidityText.text = "S  = d / t";

        speedText.text = "V  = 0 / 0";
        rapidityText.text = "S  = 0 / 0";

    }

    protected IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(3f);
        startArrow.ResetColor();
        endArrow.ResetColor();
    }

    public abstract void OnArrowPassed(Arrow arrow);
}
