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

        // TEMP
        GameManager.Instance.StartMatch();
    }

    public void RefreshBars()
    {
        CoinBar.SetAmount(GameManager.Instance.Coins, GameManager.Instance.CoinMax);
        ManaBar.SetAmount(GameManager.Instance.Mana, GameManager.Instance.ManaMax);
    }
}
