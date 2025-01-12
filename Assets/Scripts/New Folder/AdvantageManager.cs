using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdvantageManager : MonoBehaviour
{
    public TextMeshProUGUI advantageText;
    public List<string> advantageTexts;

    void Start()
    {
        if (advantageTexts != null && advantageTexts.Count > 0)
        {
            SetRandomAdvantageText();
        }
    }

    void SetRandomAdvantageText()
    {
        int randomIndex = Random.Range(0, advantageTexts.Count);
        advantageText.text = advantageTexts[randomIndex];
    }
}
