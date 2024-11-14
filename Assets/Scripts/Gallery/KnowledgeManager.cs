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

