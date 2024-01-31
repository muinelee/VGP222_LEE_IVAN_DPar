using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour
{
    public ShipDatabase shipDB;

    [SerializeField] private TMP_Text shipName;
    [SerializeField] private TMP_Text shipCost;
    [SerializeField] private Image shipImage;

    private int selectedShip = 0;

    private PlayerCreditsManager playerCredits;

    // Start is called before the first frame update
    void Start()
    {
        playerCredits = FindObjectOfType<PlayerCreditsManager>();

        for (int i = 0; i < shipDB.ShipCount; i++)
        {
            ShipSelect ship = shipDB.GetShip(i);
            ship.purchased = PlayerPrefs.GetInt("ShipPurchased_" + i, 0) == 1;
        }

        if (!PlayerPrefs.HasKey("SelectedShip"))
        {
            selectedShip = 0;
        }
        else
        {
            Load();
        }

        UpdateShip(selectedShip);
    }


    public void NextOption()
    {
        selectedShip++;

        if (selectedShip >= shipDB.ShipCount)
        {
            selectedShip = 0;
        }

        UpdateShip(selectedShip);
        Save();
    }

    public void PreviousOption()
    {
        selectedShip--;

        if(selectedShip < 0)
        {
            selectedShip = shipDB.ShipCount - 1;
        }

        UpdateShip(selectedShip);
        Save();
    }

    private void UpdateShip(int selectedShip)
    {
        ShipSelect ship = shipDB.GetShip(selectedShip);
        shipName.text = ship.shipName;
        shipCost.text = ship.shipCost.ToString();
        shipImage.sprite = ship.shipSprite;
    }

    private void Load()
    {
        selectedShip = PlayerPrefs.GetInt("SelectedShip", 0);
    }

    private void Save()
    {
        PlayerPrefs.SetInt("SelectedShip", selectedShip);
    }

    public void PurchaseShip()
    {
        ShipSelect ship = shipDB.GetShip(selectedShip);
        if (!ship.purchased && playerCredits.SpendCredits(ship.shipCost))
        {
            ship.purchased = true;
            PlayerPrefs.SetInt("ShipPurchased_" + selectedShip, ship.purchased ? 1 : 0);
        }
        else
        {
            // Insufficient funds message
        }
    }

}
