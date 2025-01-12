using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaComponent : MonoBehaviour
{
    public string value; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.AddToFormula(value); 
            Destroy(gameObject);
        }
    }
}
