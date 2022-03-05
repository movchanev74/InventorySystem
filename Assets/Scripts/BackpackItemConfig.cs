using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "backpackItemConfig", menuName = "ScriptableObjects/BackpackItemConfig")]
public class BackpackItemConfig : ScriptableObject
{
    public float itemMass;
    public string itemName;
    public string itemID;
    public ItemTypes itemType;
}