using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtilities;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

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
    public float ManaPickupWorth = 5f;

    public float ManaRegenRate = 1f;

    public int CollectedBoots;
    public int CollectedWrenches;

    public float AsteroidManaCost = 10f;
    public float TRexManaCost = 40f;
    public float QuicksandManaCost = 15f;
    public float ArmageddonManaCost = 90f;


    private float _refreshManaBarTimer;

    public Texture2D CursorImage;
    public Texture2D CursorPressedImage;
    public Canvas CursorTrailCanvas;
    private CanvasScaler _trailCanvasScaler;
    public UILineRenderer CursorTrail;

    public EndCanvasController EndCanvas;
    public Victory WinningPlayer;

    public List<Destructible> Destructibles;

    private float _victoryTimer;

    void Awake()
    {
        Destructibles = new List<Destructible>();
        Cursor.SetCursor(CursorImage, Vector2.zero, CursorMode.ForceSoftware);

        _trailCanvasScaler = CursorTrailCanvas.GetComponent<CanvasScaler>();
    }

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;

        State = GameState.Loading;
    }

    public void PostInitialize() { }

    public void Update()
    {
        // Reset main button
        if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftControl))
        {
            PoolManager.Instance.ResetPools();
            GameManager.Instance.State = GameState.Loading;
            SceneManager.LoadScene("main");
            return;
        }

        // TEMP
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            Coins = Mathf.Clamp(Coins + 10, 0f, CoinMax);
            Mana = Mathf.Clamp(Mana + 10, 0f, ManaMax);
            UIController.RefreshBars();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            EndMatch(Victory.Fixer);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            EndMatch(Victory.Destructor);
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

            CursorTrail.enabled = true;
            for (var i = 1; i < CursorTrail.Points.Length; i++)
            {
                CursorTrail.Points[i - 1] = CursorTrail.Points[i];
            }

            var xt = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
            var yt = Mathf.Clamp01(Input.mousePosition.y / Screen.height);

            var pos = new Vector2(Input.mousePosition.x * _trailCanvasScaler.referenceResolution.x / Screen.width, Input.mousePosition.y * _trailCanvasScaler.referenceResolution.y / Screen.height);

            CursorTrail.Points[CursorTrail.Points.Length - 1] = pos;
            CursorTrail.SetAllDirty();
        }
        else
        {
            CursorTrail.enabled = false;
        }

        // Update cursor clicking
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(CursorPressedImage, Vector2.zero, CursorMode.ForceSoftware);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(CursorImage, Vector2.zero, CursorMode.ForceSoftware);
        }

        // Win/lose condition
        var destroyedCount = 0;
        var maxRepairables = 0;
        foreach (var destructible in Destructibles)
        {
            if (destructible.Repairable)
            {
                maxRepairables += 1;
            }

            if (destructible.Intact == false)
            {
                destroyedCount += 1;
            }
        }

        _victoryTimer += dt;

        if (_victoryTimer > 2f && State == GameState.Match)
        {
            if (destroyedCount == 0)
            {
                EndMatch(Victory.Fixer);
            }
            else if (destroyedCount == maxRepairables)
            {
                EndMatch(Victory.Destructor);
            }
        }
    }

    public void StartMatch()
    {
        Mana = 10f;
        Coins = 0f;

        UIController.RefreshBars();

        // Destroy half of destructibles
        var destroy = false;
        foreach (var destructible in Destructibles)
        {
            if (destructible.Repairable == false) continue;

            destroy = !destroy;
            if (destroy)
            {
                destructible.Destruct(false, false);
            }
        }

        CollectedBoots = 0;
        CollectedWrenches = 0;
    }

    public void EndMatch(Victory ending)
    {
        State = GameState.VictoryScreen;
        EndCanvas.gameObject.SetActive(true);

        Destructibles.Clear();

        WinningPlayer = ending;
        if (WinningPlayer == Victory.Fixer)
        {
            EndCanvas.FixerText.SetActive(true);
        }
        else
        {
            EndCanvas.DestructorText.SetActive(true);
        }
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
