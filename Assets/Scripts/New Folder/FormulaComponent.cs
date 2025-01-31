using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class FormulaComponent : MonoBehaviour
{
    public string value;
    public TextMeshProUGUI textMeshPro;
    public float rotationSpeed = 50f;
    public Material level1Material;
    public Material level2Material;
    public Material level3Material;

    private Renderer objectRenderer;

    private void Start()
    {
        textMeshPro.text = value;
        objectRenderer = GetComponent<Renderer>();
        UpdateMaterial();
    }

    private void Update()
    {
        textMeshPro.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            UIManager.Instance.AddToFormula(value);
            Destroy(gameObject);
        }
    }

    private void UpdateMaterial()
    {
        int currentLevel = GameManager.Instance.CurrentLevel;
        switch (currentLevel)
        {
            case 1:
                objectRenderer.material = level1Material;
                break;
            case 2:
                objectRenderer.material = level2Material;
                break;
            case 3:
                objectRenderer.material = level3Material;
                break;
            default:
                Debug.LogWarning("Nivel no soportado: " + currentLevel);
                break;
        }
    }
}