using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountCoin : MonoBehaviour
{
    [SerializeField] private Text _coint_text;
    void Start()
    {
        _coint_text.text = "0";
    }

    public void CoinNumber(int coin)
    {
        _coint_text.text = coin.ToString();
    }
}
