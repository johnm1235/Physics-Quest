using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyUI : MonoBehaviour
{
    public GameObject pressKeyUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             pressKeyUI.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pressKeyUI.SetActive(false);
        }
    }
}
