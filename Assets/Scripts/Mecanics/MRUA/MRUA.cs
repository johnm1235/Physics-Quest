using StatePattern;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MRUA : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerInput input;

    [Header("MRUA")]
    [SerializeField] TextMeshProUGUI speedMRUA;
    [SerializeField] TextMeshProUGUI formulaMRUA;
    [SerializeField] Slider accelerationSlider;
    [SerializeField] ButtonCheck buttonCheck;

    private bool hasAdvancedToNextSection = false;

    private float initialSpeed;
    public float elapsedTime = 2;
    private float sliderStep = 0.05f;


    public bool resetIndex = false;
    private void Awake()
    {
        this.enabled = false;
    }

    public void Start()
    {
        initialSpeed = playerController.moveSpeed;
        accelerationSlider.value = playerController.acceleration;
        formulaMRUA.text = "V = V0 + a * t";
        ResetValues();
        
    }

    public void Update()
    {
        if (Input.GetKey(input.min)) 
        {
            accelerationSlider.value -= sliderStep;
        }
        if (Input.GetKey(input.max)) 
        {
            accelerationSlider.value += sliderStep;
        }
        UpdateAcceleration(accelerationSlider.value);

        float currentSpeed = initialSpeed + playerController.acceleration * elapsedTime;
        playerController.moveSpeed = currentSpeed;

        speedMRUA.text = $"{currentSpeed:F2} m/s = {initialSpeed} m/s + {playerController.acceleration:F2} m/s² * {elapsedTime} s";

        if (buttonCheck.button && !hasAdvancedToNextSection)
        {
            speedMRUA.color = Color.green;
            GameManager.Instance.AdvanceToNextSection();
            hasAdvancedToNextSection = true;
        }

    }

    public void UpdateAcceleration(float value)
    {
        playerController.acceleration = value;
        initialSpeed = 0f; 

        
    }

    public void ResetValues()
    {
        playerController.acceleration = 0;
        accelerationSlider.value = 0;
        initialSpeed = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
         //   GameManager.Instance.UnlockCursor();
            this.enabled = true;
            accelerationSlider.onValueChanged.AddListener(UpdateAcceleration);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  GameManager.Instance.BlockCursor();
            Debug.Log("Player has exited the MRUA section");
            accelerationSlider.onValueChanged.RemoveListener(UpdateAcceleration);
            playerController.acceleration = 20;
            playerController.moveSpeed = 5;
            this.enabled = false;
        }
    }

    public void ResetPlayerValues()
    {
        playerController.acceleration = 20;
        playerController.moveSpeed = 5; 
        accelerationSlider.value = 0;
        initialSpeed = 0;
        resetIndex = true;
    }

}
