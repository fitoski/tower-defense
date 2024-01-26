using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Building Shop", menuName = "Building System/Building Shop")]
public class BuildingShop : ScriptableObject
{
    public List<TurretData> availableTurrets = new List<TurretData>();
    public List<WallData> availableWalls = new List<WallData>();
}
