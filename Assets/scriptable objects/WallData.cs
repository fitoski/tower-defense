using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Data", menuName = "Building System/Wall Data")]
public class WallData : ScriptableObject
{
    public string wallName;
    public Sprite wallImage;
    public int cost;
}