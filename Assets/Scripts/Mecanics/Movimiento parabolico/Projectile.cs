using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _InitialVelocity;
    [SerializeField] private float _Angle;
    [SerializeField] LineRenderer _Line;
    [SerializeField] private float _Step;
    [SerializeField] Transform _FirePoint;
    [SerializeField] private float _TotalTime = 2f; // Define el tiempo total como una variable

    private void Update()
    {
        float angle = _Angle * Mathf.Deg2Rad;
        DrawPath(_InitialVelocity, angle, _Step);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(_InitialVelocity, angle)); // Pasa el ángulo en radianes
        }
    }

    private void DrawPath(float v0, float angle, float step)
    {
        step = Mathf.Max(0.001f, step); // Asegúrate de que el paso no sea menor que 0.001
        _Line.positionCount = (int)(_TotalTime / step) + 2;
        int count = 0;
        for (float t = 0; t < _TotalTime; t += step)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            _Line.SetPosition(count, _FirePoint.position + new Vector3(x, y, 0));
            count++;
        }
        float xFinal = v0 * _TotalTime * Mathf.Cos(angle);
        float yFinal = v0 * _TotalTime * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(_TotalTime, 2);
        _Line.SetPosition(count, _FirePoint.position + new Vector3(xFinal, yFinal, 0));
    }

    IEnumerator Coroutine_Movement(float v0, float angle)
    {
        float t = 0;
        while (t < _TotalTime) // Usa el mismo tiempo total que en DrawPath
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _FirePoint.position + new Vector3(x, y, 0);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
