using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShipDatabase : ScriptableObject
{
    public ShipSelect[] ship;

    public int ShipCount
    {
        get
        {
            return ship.Length;
        }
    }

    public ShipSelect GetShip(int index)
    {
        return ship[index];
    }
}
