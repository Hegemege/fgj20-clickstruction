using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtilities;

public class GameManager : GenericManager<GameManager>, ILoadedManager
{
    public AnimationCurve PartRepairPositionAnimation;
    public AnimationCurve PartRepairRotationAnimation;

    public CameraController CameraController;

    public GameUICanvasController UIController;

    public GameState State;


    public float ManaMax = 100f;
    public float CoinMax = 100f;


    public float Mana = 10f;
    public float Coins = 0f;

    public float CoinPickupWorth = 1f;
    public float ManaPickupWorth = 1f;

    public float ManaRegenRate = 0.5f;


    private float _refreshManaBarTimer;

    public Texture2D CursorImage;

    void Awake()
    {
        Cursor.SetCursor(CursorImage, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;

        State = GameState.Loading;
    }

    public void PostInitialize() { }

    public void Update()
    {
        // TEMP
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            Coins = Mathf.Clamp(Coins + 10, 0f, CoinMax);
            Mana = Mathf.Clamp(Mana + 10, 0f, ManaMax);
            UIController.RefreshBars();
        }
#endif

        var dt = Time.deltaTime;
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "main" && State != GameState.Match && State != GameState.VictoryScreen)
        {
            State = GameState.Match;
            StartMatch();
        }

        if (State == GameState.Match)
        {
            Mana = Mathf.Clamp(Mana + ManaRegenRate * dt, 0f, ManaMax);

            _refreshManaBarTimer += dt;
            if (_refreshManaBarTimer > 1f)
            {
                _refreshManaBarTimer = 0f;
                UIController.RefreshManaBar();
            }
        }
    }

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
