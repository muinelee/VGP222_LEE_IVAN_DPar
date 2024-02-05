using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShipManager : MonoBehaviour
{
    public ShipDatabase shipDB;
    public TMP_Text shipName, shipCost;
    public Image shipImage;
    public Button purchaseButton;
    public TMP_Text purchaseNotificationText;

    [SerializeField] private PlayerCreditsManager playerCredits;

    private HashSet<int> purchasedShips = new HashSet<int>();
    private int selectedShipIndex = 0;

    void Start()
    {
        LoadPurchasedShips();
        DisplayShip(selectedShipIndex); // Display the first ship by default
    }

    private void LoadPurchasedShips()
    {
        purchasedShips.Add(0); // Default ship is always purchased
        for (int i = 1; i < shipDB.ShipCount; i++)
        {
            if (PlayerPrefs.GetInt($"ShipPurchased_{i}", 0) == 1)
            {
                purchasedShips.Add(i);
            }
        }
    }

    public void SelectNextShip()
    {
        selectedShipIndex = (selectedShipIndex + 1) % shipDB.ShipCount;
        DisplayShip(selectedShipIndex);
    }

    public void SelectPreviousShip()
    {
        selectedShipIndex--;
        if (selectedShipIndex < 0) selectedShipIndex = shipDB.ShipCount - 1;
        DisplayShip(selectedShipIndex);
    }

    private void DisplayShip(int index)
    {
        ShipSelect ship = shipDB.GetShip(index);
        shipName.text = ship.shipName;
        shipCost.text = $"Cost: {ship.shipCost}";
        shipImage.sprite = ship.shipSprite;
        shipImage.color = purchasedShips.Contains(index) ? Color.white : Color.grey;

        // Enable purchase button if the ship is not already purchased
        purchaseButton.interactable = !purchasedShips.Contains(index);
    }

    public void PurchaseShip()
    {
        if (purchasedShips.Contains(selectedShipIndex))
        {
            purchaseNotificationText.text = "Ship already purchased.";
            return;
        }

        ShipSelect ship = shipDB.GetShip(selectedShipIndex);
        if (playerCredits.SpendCredits(ship.shipCost))
        {
            purchasedShips.Add(selectedShipIndex);
            PlayerPrefs.SetInt($"ShipPurchased_{selectedShipIndex}", 1);
            DisplayShip(selectedShipIndex);
            purchaseNotificationText.text = $"{ship.shipName} Purchased!";
        }
        else
        {
            purchaseNotificationText.text = "Insufficient Credits.";
        }
    }

    // Call this function when transitioning to the gameplay scene
    public void LoadSelectedShipForGameplay()
    {
        // If the last viewed ship is not purchased, load the default ship
        if (!purchasedShips.Contains(selectedShipIndex))
        {
            selectedShipIndex = 0; // Default ship index
        }

        PlayerPrefs.SetInt("SelectedShip", selectedShipIndex); // Save the ship to be loaded in gameplay
        // Here you would typically load the gameplay scene or ensure the game uses this selected ship
    }
}
