using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class Projectile : MonoBehaviour
    {
        public float maxDistance = 50f;  // Distancia máxima que puede recorrer el proyectil
        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            // Destruir el proyectil si ha recorrido la distancia máxima
            if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            // Destruir el proyectil al impactar con otro objeto
            Destroy(gameObject);
        }
    }
}
