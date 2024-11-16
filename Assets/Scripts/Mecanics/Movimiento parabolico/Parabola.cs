using StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Projectile projectile;





    public void Awake()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        playerController.enabled = true;
        projectile.enabled = false;


    }

    public void Start()
    {
        if (secondaryCamera == null)
        {
            Debug.LogError("Main camera is not assigned.");
        }
        GameManager.Instance.BlockCursor();
        projectile.enabled = false;

    }

    public void ActivateMainCamera()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        projectile.enabled = false;


    }

    public void ActivateSecondaryCamera()
    {
        mainCamera.enabled = false;
        secondaryCamera.enabled = true;
        projectile.enabled = true;
        GameManager.Instance.UnlockCursor();
    }

    public void ActivatePlayerController()
    {
        playerController.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateSecondaryCamera();
            playerController.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateMainCamera();
            playerController.enabled = true;
        }
    }
}
