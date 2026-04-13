using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;

    Bank bank;

    public void RewardGold()
    {
        if (bank == null)
        {
            return;
        }
        bank.Deposit(goldReward);
    }

    public void penalizeGold()
    {
        if (bank == null)
        {
            return;
        }
        bank.Withdraw(goldPenalty);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bank = FindFirstObjectByType<Bank>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
