using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectileSettings : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider angleSlider;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI angleText;

    public float speed { get; private set; }
    public float angle { get; private set; }

    private void Start()
    {
        speedSlider.onValueChanged.AddListener(UpdateSpeed);
        angleSlider.onValueChanged.AddListener(UpdateAngle);
    }

    private void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
        speedText.text = $"Speed: {speed:F2}";
    }

    private void UpdateAngle(float newAngle)
    {
        angle = newAngle;
        angleText.text = $"Angle: {angle:F2}°";
    }
}
