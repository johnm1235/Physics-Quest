using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _initialVelocity;
    [SerializeField] private float _angle;
    [SerializeField] LineRenderer _line;
    [SerializeField] private float _step;
    [SerializeField] Transform _firePoint;
    [SerializeField] private float _totalTime = 2f; // Define el tiempo total como una variable

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider angleSlider;

    private void Update()
    {
        // Actualiza la velocidad y el ángulo con los valores de los sliders
        _initialVelocity = speedSlider.value;
        _angle = angleSlider.value;

        float angle = _angle * Mathf.Deg2Rad;
        DrawPath(_initialVelocity, angle, _step);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(_initialVelocity, angle)); // Pasa el ángulo en radianes
        }
    }

    private void DrawPath(float v0, float angle, float step)
    {
        step = Mathf.Max(0.001f, step);
        _line.positionCount = (int)(_totalTime / step) + 2;
        int count = 0;
        for (float t = 0; t < _totalTime; t += step)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            _line.SetPosition(count, _firePoint.position + new Vector3(x, y, 0));
            count++;
        }
        float xFinal = v0 * _totalTime * Mathf.Cos(angle);
        float yFinal = v0 * _totalTime * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(_totalTime, 2);
        _line.SetPosition(count, _firePoint.position + new Vector3(xFinal, yFinal, 0));
    }

    IEnumerator Coroutine_Movement(float v0, float angle)
    {
        float t = 0;
        while (t < _totalTime) // Usa el mismo tiempo total que en DrawPath
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            Vector3 newPosition = _firePoint.position + new Vector3(x, y, 0);
            transform.position = newPosition;
            t += Time.deltaTime;
            yield return null;
        }
    }
}
