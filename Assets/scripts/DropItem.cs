using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public string itemName;
    public Enums.ItemType itemType; 
    public GameObject itemPrefab;
    [Range(0f, 100f)] public int dropChance;
}