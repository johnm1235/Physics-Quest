using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FormulaComponent : MonoBehaviour
{
    public string value;
    public TextMeshProUGUI textMeshPro;
    public float rotationSpeed = 50f;

    private void Start()
    {
        textMeshPro.text = value;
    }

    private void Update()
    {
        textMeshPro.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.AddToFormula(value);
            Destroy(gameObject);
        }
    }
}
