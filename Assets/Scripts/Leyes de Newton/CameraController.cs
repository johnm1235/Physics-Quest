using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Referencia al transform del jugador
    public Vector3 offset;    // Desplazamiento de la cámara respecto al jugador

    // Start is called before the first frame update
    void Start()
    {
        // Si no se ha asignado un offset, se establece uno por defecto
        if (offset == Vector3.zero)
        {
            offset = new Vector3(0, 5, -10);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Calcula la posición deseada de la cámara
        Vector3 desiredPosition = player.position + offset;
        transform.position = desiredPosition;

        // Opcional: Si quieres que la cámara siempre mire al jugador
        // transform.LookAt(player);
    }
}
