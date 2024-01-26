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

    // Call this method when a player dies to clear equipped stash items
    public void ClearEquippedStashItems()
    {
        // Implement logic to remove equipped stash items
    }

    // Implement additional methods as needed
}