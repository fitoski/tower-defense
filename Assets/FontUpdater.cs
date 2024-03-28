using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FontUpdater : MonoBehaviour
{
    public Font newFont;
    public TMP_FontAsset newTMPFont;

    void Start()
    {
        UpdateAllTextFonts();
    }

    void UpdateAllTextFonts()
    {
        Text[] texts = FindObjectsOfType<Text>();
        foreach (Text text in texts)
        {
            text.font = newFont;
        }

        TextMeshProUGUI[] tmpTexts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI tmpText in tmpTexts)
        {
            tmpText.font = newTMPFont;
        }
    }
}