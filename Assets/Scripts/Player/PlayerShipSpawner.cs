using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipSpawner : MonoBehaviour
{
    public ShipDatabase shipDB;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        int selectedShipIndex = PlayerPrefs.GetInt("SelectedShip", 0);
        ShipSelect selectedShip = shipDB.GetShip(selectedShipIndex);
        Instantiate(selectedShip.shipPrefab, spawnPoint.position, Quaternion.identity);
    }
}
