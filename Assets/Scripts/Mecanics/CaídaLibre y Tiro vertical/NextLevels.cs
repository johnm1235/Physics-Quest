using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevels : MonoBehaviour
{
    [SerializeField] private ButtonCheck buttonCheck;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonCheck.button)
        {
            GameManager.Instance.AdvanceToNextSection();
        }
    }
}
