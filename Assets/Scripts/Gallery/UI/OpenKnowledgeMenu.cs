using StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenKnowledgeMenu : MonoBehaviour
{
    [SerializeField] private GameObject knowledgeMenu;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private AudioClip buttonClickSound;

    private void Update()
    {
        if (playerInput.IsMenuOpen)
        {
            bool isActive = !knowledgeMenu.activeSelf;
            knowledgeMenu.SetActive(isActive);

            if (isActive)
            {
                AudioManager.Instance.PlaySFX(buttonClickSound);
                GameManager.Instance.UnlockCursor();
            }
            else
            {
                AudioManager.Instance.PlaySFX(buttonClickSound);
                GameManager.Instance.BlockCursor();
            }
        }
    }
}
