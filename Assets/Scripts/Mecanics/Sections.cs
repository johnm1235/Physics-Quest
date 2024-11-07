using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sections : MonoBehaviour
{
    public GameObject canvasSpeed;

    public void Start()
    {
        canvasSpeed.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasSpeed.SetActive(true); 
         //   GameManager.Instance.CompleteSection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasSpeed.SetActive(false); 
        }
    }
}
