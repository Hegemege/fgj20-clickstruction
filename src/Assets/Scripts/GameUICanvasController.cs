using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUICanvasController : MonoBehaviour
{
    public ManaBar CoinBar;
    public ManaBar ManaBar;

    void Start()
    {
        GameManager.Instance.UIController = this;
    }

    public void RefreshBars()
    {
        RefreshCoinBar();
        RefreshManaBar();
    }

    public void RefreshCoinBar()
    {
        CoinBar.SetAmount(GameManager.Instance.Coins, GameManager.Instance.CoinMax);
    }

    public void RefreshManaBar()
    {
        ManaBar.SetAmount(GameManager.Instance.Mana, GameManager.Instance.ManaMax);
    }
}
