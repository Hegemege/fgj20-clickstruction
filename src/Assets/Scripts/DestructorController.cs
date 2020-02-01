using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtilities;

public class DestructorController : MonoBehaviour
{
    private DestructorAbility _currentAbility = DestructorAbility.None;

    private EventSystem _eventSystem;

    void Awake()
    {
        _eventSystem = GetComponentInChildren<EventSystem>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    private void OnClick()
    {
        if (_eventSystem.IsPointerOverGameObject()) return;
        Debug.Log("HERE");
    }
}