using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtilities;

public class DestructorController : MonoBehaviour
{
    void Awake()
    {

    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        this.Log(ctx.phase);
    }
}
