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

    public GameObject door; 
    public GameObject leftVelocityIndicator; 
    public GameObject leftSpeedIndicator; 
    public GameObject rightVelocityIndicator;
    public GameObject rightSpeedIndicator;
    public GameObject door1;
    public GameObject door2;
    [SerializeField] private ButtonCheck buttonCheck;

    public RightCalculator rightCalculator;
    public LeftCalculator leftCalculator;

    private bool sectionCompleted = false; 
    private Coroutine checkCoroutine;


    private void Start()
    {
        rightCalculator.enabled = false;
        leftCalculator.enabled = false;
    }

    public void Update()
    {
        if (buttonCheck.button && !sectionCompleted)
        {
            GameManager.Instance.AdvanceToNextSection();
            sectionCompleted = true;
        }
    }

    public void CheckConditions()
    {


        if (leftVelocity && leftSpeed)
        {
            OpenDoor1();
            if (rightSpeed && rightVelocity)
            {
                OpenDoor2();
                OpenDoor();
              //  sectionCompleted = true;

                if (checkCoroutine == null)
                {
                    //checkCoroutine = StartCoroutine(WaitAndCompleteSection());
                }

            }
            else
            {
                CloseDoor();
            }

        }
        else
        {
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
                checkCoroutine = null;
            }
            CloseDoor1();
        }
    }

    private IEnumerator WaitAndCompleteSection()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.CompleteSection();

    }

    private void OpenDoor()
    {
        door.SetActive(false);
    }

    private void OpenDoor1()
    {
        door1.SetActive(false);
    }
    private void OpenDoor2()
    {
        door2.SetActive(false);
    }

    private void CloseDoor()
    {
        door.SetActive(true);
        door2.SetActive(true);
    }

    private void CloseDoor1()
    {
        door1.SetActive(true);
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
        }
        else if (other.CompareTag("RightPath"))
        {
            rightCalculator.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
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
