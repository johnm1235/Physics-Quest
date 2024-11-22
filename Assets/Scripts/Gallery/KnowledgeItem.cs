using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeItem : MonoBehaviour
{
    [SerializeField] private string knowledgeTitle;
    [SerializeField] private Sprite knowledgeContent;

    [SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 100f, 0f); 

    public string Title => knowledgeTitle;
    public Sprite Content => knowledgeContent;

    private void Update()
    {
        
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KnowledgeManager.Instance.AddKnowledge(knowledgeTitle, knowledgeContent);
            FindObjectOfType<LevelOneManager>().PlayItemPickupSound();
            ShowCollectionEffect();
            Destroy(gameObject);
        }
    }

    private void ShowCollectionEffect()
    {
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
