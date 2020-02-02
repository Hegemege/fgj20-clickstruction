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

    private float _cost;

    void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
    }

    public void Initialize()
    {
        BootsPickup.SetActive(Type == PickupType.Boots);
        WrenchPickup.SetActive(Type == PickupType.Wrench);
        ShieldPickup.SetActive(Type == PickupType.Shield);

        switch (Type)
        {
            case PickupType.Boots:
                _cost = BootsCost;
                break;
            case PickupType.Wrench:
                _cost = WrenchCost;
                break;
            case PickupType.Shield:
                _cost = ShieldCost;
                break;
        }
        PriceText.text = FloatStringCache.Get(_cost, 0);
    }

    public void Reset()
    {

    }

}
