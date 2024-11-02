using StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKnowledgeMenu : MonoBehaviour
{
    [SerializeField] private GameObject knowledgeMenu;
    [SerializeField] private PlayerInput playerInput;

    private void Update()
    {
        if (playerInput.IsMenuOpen)
        {
            knowledgeMenu.SetActive(!knowledgeMenu.activeSelf);
        }
    }
}

