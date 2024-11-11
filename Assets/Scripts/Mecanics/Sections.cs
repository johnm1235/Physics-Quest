using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sections : MonoBehaviour
{
    public GameObject canvasSpeed;
    public GameObject objCalculate;

    public void Start()
    {
        canvasSpeed.SetActive(false);
        objCalculate.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasSpeed.SetActive(true);
            objCalculate.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvasSpeed.SetActive(false);
            objCalculate.SetActive(false);
        }
    }
}
