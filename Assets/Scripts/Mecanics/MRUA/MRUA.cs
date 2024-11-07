using StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MRUA : MonoBehaviour
{
    [Header("MRUA Settings")]
    [SerializeField] private float mruaAcceleration = 2f; 
    [SerializeField] private float mruaMaxTime = 5f;      
    private float mruaCurrentTime;                       
    private float currentSpeed;                           

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI accelerationText;
    [SerializeField] private TextMeshProUGUI timeText;

    private bool isInMRUASection = false;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isInMRUASection)
        {
            UpdateMRUAMechanics();
            UpdateUI();
        }
    }

    public void StartMRUA()
    {
        isInMRUASection = true;
        mruaCurrentTime = 0f;
        currentSpeed = 0f;
    }

    public void StopMRUA()
    {
        isInMRUASection = false;
        ResetUI();
    }

    private void UpdateMRUAMechanics()
    {
        currentSpeed += mruaAcceleration * Time.deltaTime;

        playerController.MRUA(currentSpeed);

        mruaCurrentTime += Time.deltaTime;
        if (mruaCurrentTime >= mruaMaxTime)
        {
            Debug.Log("Tiempo agotado en MRUA.");
            StopMRUA();
            GameManager.Instance.RestartSection();
        }
    }

    private void UpdateUI()
    {
        if (velocityText != null) velocityText.text = $"Velocidad: {currentSpeed:F2}";
        if (accelerationText != null) accelerationText.text = $"Aceleración: {mruaAcceleration:F2}";
        if (timeText != null) timeText.text = $"Tiempo restante: {Mathf.Max(0, mruaMaxTime - mruaCurrentTime):F2}";
    }

    private void ResetUI()
    {
        if (velocityText != null) velocityText.text = "Velocidad: 0";
        if (accelerationText != null) accelerationText.text = "Aceleración: 0";
        if (timeText != null) timeText.text = "Tiempo restante: -";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CompleteSection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
