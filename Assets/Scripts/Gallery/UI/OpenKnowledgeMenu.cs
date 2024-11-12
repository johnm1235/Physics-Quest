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
            bool isActive = !knowledgeMenu.activeSelf;
            knowledgeMenu.SetActive(isActive);

            if (isActive)
            {
                GameManager.Instance.UnlockCursor();
            }
            else
            {
                GameManager.Instance.BlockCursor();
            }
        }
    }
}
