using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public TextMeshProUGUI purse;

    public int count;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        purse.SetText($"$:{count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        EnemyDemo.addCoins += addCoins;
    }
    
    private void addCoins(int coins)
    {
        count += coins;
        purse.SetText($"$:{count}");
    }

    private void OnDisable()
    {
        EnemyDemo.addCoins -= addCoins;
    }
}
