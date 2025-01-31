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
    private UIManager uiManager;

    private void Start()
    {
        textMeshPro.text = value;
        objectRenderer = GetComponent<Renderer>();

        // Buscar y asignar el UIManager
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("No se encontró UIManager en la escena.");
            return;
        }

        UpdateMaterial();
    }

    private void Update()
    {
        textMeshPro.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();

        // Verificar si el objeto con el que colisiona es el jugador y es de su propiedad
        if (other.CompareTag("Player") && photonView != null && photonView.IsMine)
        {
            uiManager.AddToFormula(value);

            // Destruir el objeto en la red para todos los jugadores, asegurándonos de que el cliente sea el propietario o el MasterClient
            if (photonView.IsMine || PhotonNetwork.IsMasterClient)
            {
                Destroy(gameObject); // Destruir el objeto de forma correcta en la red
            }
            else
            {
                Debug.LogWarning("No tienes permisos para destruir este objeto.");
            }
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
