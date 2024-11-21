using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformActive : MonoBehaviour
{

    [SerializeField] private GameObject plataform;
    [SerializeField] private GameObject plataformObs; 

    private void Start()
    {
        if (plataform != null)
        {
            plataform.SetActive(false); 
        }
        if (plataformObs != null)
        {
            plataformObs.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (plataform != null)
            {
                plataform.SetActive(true); 
            }
            if (plataformObs != null)
            {
                plataformObs.SetActive(false);
            }
        }
    }

    public void ResetAll()
    {

            if (plataform != null)
            {
                plataform.SetActive(false);
            }
            if (plataformObs != null)
            {
                plataformObs.SetActive(true);
            }
       

    }
}
