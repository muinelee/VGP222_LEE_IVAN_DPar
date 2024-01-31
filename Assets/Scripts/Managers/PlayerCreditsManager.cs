using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreditsManager : MonoBehaviour
{
    public int credits;

    void Start()
    {
        LoadCredits();
    }

    public void AddCredits(int amount)
    {
        credits += amount;
        SaveCredits();
    }

    public bool SpendCredits(int amount)
    {
        if (credits >= amount)
        {
            credits -= amount;
            SaveCredits();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void LoadCredits()
    {
        credits = PlayerPrefs.GetInt("PlayerCredits", 0);
    }

    private void SaveCredits()
    {
        PlayerPrefs.SetInt("PlayerCredits", credits);
    }
}
