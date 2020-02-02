using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityUtilities;

public class PickupController : MonoBehaviour, IResetableBehaviour
{
    public PickupType Type;

    public GameObject BootsPickup;
    public GameObject WrenchPickup;
    public GameObject ShieldPickup;

    private Canvas _canvas;
    public TextMeshProUGUI PriceText;
    public float BootsCost = 20;
    public float WrenchCost = 15;
    public float ShieldCost = 80;

    [HideInInspector]
    public float Cost;

    public bool InitializeOnAwake;
    public float CanvasRotationSpeed = 180f;

    void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();

        if (InitializeOnAwake)
        {
            Initialize();
        }
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        _canvas.transform.Rotate(Vector3.up, CanvasRotationSpeed * dt);
    }

    public void Initialize()
    {
        BootsPickup.SetActive(Type == PickupType.Boots);
        WrenchPickup.SetActive(Type == PickupType.Wrench);
        ShieldPickup.SetActive(Type == PickupType.Shield);

        switch (Type)
        {
            case PickupType.Boots:
                Cost = BootsCost;
                break;
            case PickupType.Wrench:
                Cost = WrenchCost;
                break;
            case PickupType.Shield:
                Cost = ShieldCost;
                break;
        }
        PriceText.text = FloatStringCache.Get(Cost, 0);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

}
