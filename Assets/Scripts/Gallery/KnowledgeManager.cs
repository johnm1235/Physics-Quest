using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance;
    private List<KnowledgeEntry> knowledgeEntries = new List<KnowledgeEntry>();

    // Define el m�ximo de energ�a que se puede recolectar por nivel
    [SerializeField] private int maxEnergyPerLevel = 10;
    public int MaxEnergyPerLevel => maxEnergyPerLevel;

    // Referencia al objeto que deseas activar/desactivar
    [SerializeField] private GameObject barrera1;
    [SerializeField] private GameObject barrera2;
    [SerializeField] private GameObject barrera3;
    [SerializeField] private GameObject barrera4;
    [SerializeField] private GameObject barrera5;
    [SerializeField] private GameObject barrera6;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKnowledge(string title, Sprite content)
    {
        if (knowledgeEntries.Count < maxEnergyPerLevel) // Verifica si no se ha alcanzado el m�ximo
        {
            knowledgeEntries.Add(new KnowledgeEntry(title, content));
            // Llama a EnergyBar para actualizar
            FindObjectOfType<EnergyBar>().UpdateEnergyBar();

            // L�gica para activar/desactivar el objeto basado en el t�tulo
            if (title == "Velocidad")
            {
                barrera1.SetActive(false);
            }
            if (title == "Rapidez")
            {
                barrera2.SetActive(false);
            }
            if (title == "MRU")
            {
                barrera3.SetActive(false);
            }
            if (title == "MRUA")
            {
                barrera4.SetActive(false);
            }
            if (title == "Ca�da Libre")
            {
                barrera5.SetActive(false);
            }
            if (title == "Movimiento parab�lico")
            {
                barrera6.SetActive(false);
            }
        }
        else
        {
            Debug.Log("M�ximo de energ�a alcanzado para este nivel.");
        }
    }

    public List<KnowledgeEntry> GetAllKnowledge()
    {
        return knowledgeEntries;
    }
}

[System.Serializable]
public class KnowledgeEntry
{
    public string Title;
    public Sprite Content;

    public KnowledgeEntry(string title, Sprite content)
    {
        Title = title;
        Content = content;
    }
}
