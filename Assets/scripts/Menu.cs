using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI goldUI;

    private void OnGUI()
    {
        goldUI.text = GameManager.main.Gold.ToString();
    }

    public void SetSelected()
    {

    }
}