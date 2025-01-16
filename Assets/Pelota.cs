using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota : MonoBehaviour
{
    // Nueva variable para el nivel del jugador
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;

    [SerializeField] private Rigidbody rb;

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontal, rb.velocity.y, vertical);

    }
}
