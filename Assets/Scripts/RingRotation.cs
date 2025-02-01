using System.Collections;
using UnityEngine;

public class RingRotation : MonoBehaviour
{
    public GameObject ring1;
    public GameObject ring2;
    public GameObject ring3;
    public Transform oscillatingObject;

    public float rotationSpeed1 = 30f;  // Velocidad de rotación para el primer anillo
    public float rotationSpeed2 = 45f;  // Velocidad de rotación para el segundo anillo
    public float rotationSpeed3 = 60f;  // Velocidad de rotación para el tercer anillo
    public float oscillationHeight = 0.5f; // Altura de la oscilación
    public float oscillationSpeed = 2f;

    private Vector3 initialOscillationPosition;

    private void Start()
    {
        initialOscillationPosition = oscillatingObject.position;
    }
    void Update()
    {
        // Rotar el primer anillo alrededor del eje X
        if (ring1 != null)
        {
            ring1.transform.Rotate(Vector3.right * rotationSpeed1 * Time.deltaTime);
        }

        // Rotar el segundo anillo alrededor del eje Y
        if (ring2 != null)
        {
            ring2.transform.Rotate(Vector3.up * rotationSpeed2 * Time.deltaTime);
        }

        // Rotar el tercer anillo alrededor del eje Z
        if (ring3 != null)
        {
            ring3.transform.Rotate(Vector3.forward * rotationSpeed3 * Time.deltaTime);
        }

        float newY = initialOscillationPosition.y + Mathf.Sin(Time.time * oscillationSpeed) * oscillationHeight;
        oscillatingObject.position = new Vector3(oscillatingObject.position.x, newY, oscillatingObject.position.z);
    }
}
