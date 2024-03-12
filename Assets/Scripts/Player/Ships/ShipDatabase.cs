using UnityEngine;

[CreateAssetMenu(fileName = "ShipDatabase", menuName = "Player Ships/ShipDatabase")]
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
