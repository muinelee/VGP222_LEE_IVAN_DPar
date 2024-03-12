using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyRewards : MonoBehaviour
{
    [SerializeField] private RewardsDatabase rewardsDB;
    [SerializeField] private TMP_Text rewardsName;
    [SerializeField] private Image rewardsImage;
    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject rewardsNotification;
    [SerializeField] private PlayerCreditsManager playerCreditsManager;
    [SerializeField] private float rewardFrequencyInSeconds = 10f;
    private bool isRewardAvailable;

    private DateTime nextRewardTime;
    private DateTime lastRewardClaimTime;
    private int rewardIndex;
    private TimeSpan rewardFrequency;

    private const string NextRewardTimeKey = "NextRewardTime";
    private const string LastRewardClaimTimeKey = "LastRewardClaimTime";
    private const string RewardIndexKey = "RewardIndex";

    private void Awake()
    {
        rewardFrequency = TimeSpan.FromSeconds(rewardFrequencyInSeconds);
    }

    private void Start()
    {
        rewardsNotification.SetActive(true);
        claimButton.onClick.AddListener(ClaimReward);
        LoadRewardState();
        CheckRewardAvailability();
        UpdateRewardUI();
    }

    private void Update()
    {
        if (DateTime.UtcNow > nextRewardTime)
        {
            rewardsNotification.SetActive(true);
            claimButton.interactable = true;
        }
        else
        {
            rewardsNotification.SetActive(false);
            claimButton.interactable = false;
        }
    }

    private void CheckRewardAvailability()
    {
        isRewardAvailable = DateTime.UtcNow > nextRewardTime;

        rewardsNotification.SetActive(isRewardAvailable);
        claimButton.interactable = isRewardAvailable;
    }

    private void ClaimReward()
    {
        RewardsGameData reward = rewardsDB.GetReward(rewardIndex);
        
        playerCreditsManager.AddCredits(reward.rewardAmount);

        rewardIndex = (rewardIndex + 1) % rewardsDB.rewardCount;
        lastRewardClaimTime = DateTime.UtcNow;
        nextRewardTime = lastRewardClaimTime.Add(rewardFrequency);
        isRewardAvailable = false;
        SaveRewardState();
        UpdateRewardUI();
    }

    private void UpdateRewardUI()
    {
        RewardsGameData reward = rewardsDB.GetReward(rewardIndex);
        rewardsName.text = reward.rewardName;
        rewardsImage.sprite = reward.rewardSprite;
    }

    private void LoadRewardState()
    {
        rewardIndex = PlayerPrefs.GetInt(RewardIndexKey, 0);
        lastRewardClaimTime = DateTime.Parse(PlayerPrefs.GetString(LastRewardClaimTimeKey, DateTime.UtcNow.ToString()));
        nextRewardTime = DateTime.Parse(PlayerPrefs.GetString(NextRewardTimeKey, DateTime.UtcNow.Add(rewardFrequency).ToString()));
    }

    private void SaveRewardState()
    {
        PlayerPrefs.SetInt(RewardIndexKey, rewardIndex);
        PlayerPrefs.SetString(LastRewardClaimTimeKey, lastRewardClaimTime.ToString());
        PlayerPrefs.SetString(NextRewardTimeKey, nextRewardTime.ToString());
        PlayerPrefs.Save();
    }
}