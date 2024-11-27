using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KnowledgeManager : MonoBehaviour
{
    public static KnowledgeManager Instance;
    private List<KnowledgeEntry> knowledgeEntries = new List<KnowledgeEntry>();

    // Define el máximo de energía que se puede recolectar por nivel
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reasignar las referencias de las barreras
        barrera1 = GameObject.FindWithTag("Barrera1");
        barrera2 = GameObject.FindWithTag("Barrera2");
        barrera3 = GameObject.FindWithTag("Barrera3");
        barrera4 = GameObject.FindWithTag("Barrera4");
        barrera5 = GameObject.FindWithTag("Barrera5");
        barrera6 = GameObject.FindWithTag("Barrera6");
    }

    public void AddKnowledge(string title, Sprite content)
    {
        if (knowledgeEntries.Count < maxEnergyPerLevel) // Verifica si no se ha alcanzado el máximo
        {
            knowledgeEntries.Add(new KnowledgeEntry(title, content));
            // Llama a EnergyBar para actualizar
            FindObjectOfType<EnergyBar>().UpdateEnergyBar();

            // Lógica para activar/desactivar el objeto basado en el título
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
            if (title == "Caída Libre")
            {
                barrera5.SetActive(false);
            }
            if (title == "Movimiento parabólico")
            {
                barrera6.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Máximo de energía alcanzado para este nivel.");
        }
    }

    public List<KnowledgeEntry> GetAllKnowledge()
    {
        return knowledgeEntries;
    }

    public void ResetKnowledge()
    {
        knowledgeEntries.Clear();

        if (barrera1 != null) barrera1.SetActive(true);
        if (barrera2 != null) barrera2.SetActive(true);
        if (barrera3 != null) barrera3.SetActive(true);
        if (barrera4 != null) barrera4.SetActive(true);
        if (barrera5 != null) barrera5.SetActive(true);
        if (barrera6 != null) barrera6.SetActive(true);
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
