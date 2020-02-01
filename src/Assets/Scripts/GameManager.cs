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
}
