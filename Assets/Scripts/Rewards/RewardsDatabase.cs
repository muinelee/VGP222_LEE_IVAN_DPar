using UnityEngine;

[CreateAssetMenu (fileName = "RewardsDatabase", menuName = "Daily Rewards/RewardsDatabase")]
public class RewardsDatabase : ScriptableObject
{
    public RewardsGameData[] rewards;

    public int rewardCount
    {
        get
        {
            return rewards.Length;
        }
    }

    public RewardsGameData GetReward(int index)
    {
        return rewards[index];
    }
}
