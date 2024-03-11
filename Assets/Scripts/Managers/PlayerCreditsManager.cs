using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCreditsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI creditsText;
    private int credits;

    void Start()
    {
        LoadCredits();
        UpdateCreditsUI();
    }

    public void AddCredits(int amount)
    {
        Debug.Log($"Adding Credits: {credits}");
        credits += amount;
        SaveCredits();
        UpdateCreditsUI();
    }

    public bool SpendCredits(int amount)
    {
        if (credits >= amount)
        {
            credits -= amount;
            SaveCredits();
            UpdateCreditsUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddTestCredits()
    {
        AddCredits(1000);
    }

    public void RemoveTestCredits()
    {
        SpendCredits(1000);
    }

    private void LoadCredits()
    {
        credits = PlayerPrefs.GetInt("PlayerCredits", 0);
    }

    private void SaveCredits()
    {
        PlayerPrefs.SetInt("PlayerCredits", credits);
    }

    private void UpdateCreditsUI()
    {
        if (creditsText != null)
        {
            creditsText.text = $"Credits: {credits}";
        }
        else
        {
            Debug.LogWarning("Credits TextMeshPro UGUI component is not assigned in the PlayerCreditsManager.");
        }
    }
}
