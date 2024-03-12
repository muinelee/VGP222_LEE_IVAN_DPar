using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private ShipDatabase shipDB;
    [SerializeField] private TMP_Text shipName, shipCost;
    [SerializeField] private Image shipImage;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TMP_Text purchaseNotificationText;

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

        // Check if the ship is already purchased
        if (purchasedShips.Contains(index))
        {
            // Change button text to "PURCHASED" and disable it
            purchaseButton.GetComponentInChildren<TMP_Text>().text = "PURCHASED";
            purchaseButton.interactable = false;
        }
        else
        {
            // If not purchased, show "BUY" and enable the button
            purchaseButton.GetComponentInChildren<TMP_Text>().text = "BUY";
            purchaseButton.interactable = true;
        }
    }

    public void PurchaseShip()
    {
        if (purchasedShips.Contains(selectedShipIndex))
        {
            ShowPurchasedNotification("Ship already purchased!");
            return;
        }

        ShipSelect ship = shipDB.GetShip(selectedShipIndex);

        if (playerCredits.SpendCredits(ship.shipCost))
        {
            purchasedShips.Add(selectedShipIndex);
            PlayerPrefs.SetInt($"ShipPurchased_{selectedShipIndex}", 1);
            DisplayShip(selectedShipIndex);
            ShowPurchasedNotification($"{ship.shipName} Purchased!");
        }
        else
        {
            ShowPurchasedNotification("Insufficient Credits!");
        }
    }

    public void ShowPurchasedNotification(string message)
    {
        purchaseNotificationText.text = message;
        purchaseNotificationText.color = new Color(purchaseNotificationText.color.r, purchaseNotificationText.color.g, purchaseNotificationText.color.b, 1);

        StartCoroutine(FadeTextToZeroAlpha(2.5f, purchaseNotificationText, 2.5f));
    }

    private IEnumerator FadeTextToZeroAlpha(float duration, TMP_Text text, float delayBeforeFadeStarts)
    {
        yield return new WaitForSeconds(delayBeforeFadeStarts);

        Color originalColor = text.color;

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    public void LoadSelectedShipForGameplay()
    {
        // If the last viewed ship is not purchased, load the default ship
        if (!purchasedShips.Contains(selectedShipIndex))
        {
            selectedShipIndex = 0;
        }

        PlayerPrefs.SetInt("SelectedShip", selectedShipIndex);
    }
}
