using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class GameManager : GenericManager<GameManager>, ILoadedManager
{
    public AnimationCurve PartRepairPositionAnimation;
    public AnimationCurve PartRepairRotationAnimation;

    public CameraController CameraController;

    public GameUICanvasController UIController;


    public float ManaMax = 100f;
    public float CoinMax = 100f;


    public float Mana = 10f;
    public float Coins = 0f;

    public float CoinPickupWorth = 1f;
    public float ManaPickupWorth = 1f;


    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;
    }

    public void PostInitialize() { }

    public void StartMatch()
    {
        Mana = 10f;
        Coins = 0f;

        UIController.RefreshBars();
    }

    public void PickupCoin()
    {
        Coins = Mathf.Clamp(Coins + CoinPickupWorth, 0f, CoinMax);
        UIController.RefreshBars();
    }

    public void PickupMana()
    {
        Mana = Mathf.Clamp(Mana + ManaPickupWorth, 0f, ManaMax);
        UIController.RefreshBars();
    }

    public bool SpendCoins(float amount)
    {
        if (amount > Coins) return false;

        Coins -= amount;
        UIController.RefreshBars();
        return true;
    }

    public bool SpendMana(float amount)
    {
        if (amount > Mana) return false;

        Mana -= amount;
        UIController.RefreshBars();
        return true;
    }
}
