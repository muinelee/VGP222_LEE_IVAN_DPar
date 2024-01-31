using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShipSelect
{
    public GameObject shipPrefab;
    public Sprite shipSprite;
    public string shipName;
    public int shipCost;
    public bool purchased;
}
