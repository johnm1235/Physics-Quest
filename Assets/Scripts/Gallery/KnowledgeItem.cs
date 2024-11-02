using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeItem : MonoBehaviour
{
    [SerializeField] private string knowledgeTitle;
    [TextArea][SerializeField] private string knowledgeContent;

    public string Title => knowledgeTitle;
    public string Content => knowledgeContent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Agrega el conocimiento al inventario del jugador
            KnowledgeManager.Instance.AddKnowledge(knowledgeTitle, knowledgeContent);
            // Muestra un efecto de partículas al recogerlo
            ShowCollectionEffect();
            // Destruye el objeto
            Destroy(gameObject);
        }
    }

    private void ShowCollectionEffect()
    {
        // Agrega la lógica para mostrar efectos visuales (ej. partículas)
        // Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
    }
}

