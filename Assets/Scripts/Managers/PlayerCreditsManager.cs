using TMPro;
using UnityEngine;

public class PlayerCreditsManager : MonoBehaviour
{
    public static PlayerCreditsManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI creditsText;
    private int credits;
    
    private SaveLoadManager saveLoadManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveLoadManager = GetComponent<SaveLoadManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadCredits();
        UpdateCreditsUI();
    }

    public void AddCredits(int amount)
    {
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
        SaveLoadManager.PlayerCreditsData data = saveLoadManager.LoadCredits();
        if (data != null)
        {
            credits = data.totalCredits;
        }
        else
        {
            credits = 0;
        }
    }

    private void SaveCredits()
    {
        saveLoadManager.SaveCredits(credits);
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
