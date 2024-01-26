using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Turret Data", menuName = "Building System/Turret Data")]
public class TurretData : ScriptableObject
{
    public string turretName;
    public Sprite turretImage;
    public int cost;
}
