using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashManager : MonoBehaviour
{
    public List<StashItem> StashItems = new List<StashItem>();

    public void AddToStash(StashItem item)
    {
        StashItems.Add(item);
    }

    public void ClearEquippedStashItems()
    {
    }

}