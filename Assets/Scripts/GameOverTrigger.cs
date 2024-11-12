using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private MRUUI mruUI;

    private void Start()
    {
        GameObject mruUIObject = GameObject.FindWithTag("MRUUI");
        if (mruUIObject != null)
        {
            mruUI = mruUIObject.GetComponent<MRUUI>();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            GameManager.Instance.RestartSection();
        }
    }
}
