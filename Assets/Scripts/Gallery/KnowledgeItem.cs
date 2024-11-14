using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeItem : MonoBehaviour
{
    [SerializeField] private string knowledgeTitle;
    [SerializeField] private Sprite knowledgeContent; // Aseg�rate de que est� marcado como [SerializeField]

    public string Title => knowledgeTitle;
    public Sprite Content => knowledgeContent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Agrega el conocimiento al inventario del jugador
            KnowledgeManager.Instance.AddKnowledge(knowledgeTitle, knowledgeContent);
            // Muestra un efecto de part�culas al recogerlo
            ShowCollectionEffect();
            // Destruye el objeto
            Destroy(gameObject);
        }
    }

    private void ShowCollectionEffect()
    {
        // Agrega la l�gica para mostrar efectos visuales (ej. part�culas)
        // Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
    }
}
