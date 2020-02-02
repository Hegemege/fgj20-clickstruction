using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour, IResetableBehaviour
{
    public PickupType Type;

    public GameObject BootsPickup;
    public GameObject WrenchPickup;
    public GameObject ShieldPickup;


    public void Initialize()
    {
        BootsPickup.SetActive(Type == PickupType.Boots);
        WrenchPickup.SetActive(Type == PickupType.Wrench);
        ShieldPickup.SetActive(Type == PickupType.Shield);
    }

    public void Reset()
    {

    }

}
