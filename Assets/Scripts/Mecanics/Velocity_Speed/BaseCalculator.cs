using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class BaseCalculator : MonoBehaviour
{
    [Header("Indicators")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rapidityText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI displacementText;
    public TextMeshProUGUI time;
    public TextMeshProUGUI time2;

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
        speedText.text = speed.ToString("F2");
        rapidityText.text = rapidity.ToString("F2");
        distanceText.text = totalDistance.ToString("F2");
        displacementText.text = netDisplacement.ToString("F2");
        time.text = timeElapsed.ToString("F2");
        time2.text = timeElapsed.ToString("F2");
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
        speedText.text = "V";
        rapidityText.text = "S";
        distanceText.text = "d";
        displacementText.text = "Ax";
        time.text = "t";
        time2.text = "t";
    }

    protected IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(3f);
        startArrow.ResetColor();
        endArrow.ResetColor();
    }

    public abstract void OnArrowPassed(Arrow arrow);
}
